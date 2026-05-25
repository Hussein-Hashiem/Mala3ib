using Mala3ib.BLL.Abstraction;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Mala3ib.BLL.Service.Implementation
{
    public class FieldService : IFieldService
    {
        private readonly IFieldRepo _fieldRepo;
        private readonly IFieldOwnerRepo _fieldOwnerRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FieldService> _logger;
        public FieldService(IFieldRepo fieldRepo, IFieldOwnerRepo fieldOwnerRepo, IWebHostEnvironment webHostEnvironment, ILogger<FieldService> logger)
        {
            _fieldRepo = fieldRepo;
            _fieldOwnerRepo = fieldOwnerRepo;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<Result<FieldResponseDto>> AddAsync(AddFieldRequestDto request, string userId, CancellationToken cancellation = default)
        {
            var ownerId = await GetOwnerIdByUserIdAsync(userId, cancellation);

            if (ownerId.IsFailure)
                return Result.Failure<FieldResponseDto>(ownerId.Error);

            var field = new Field
            {
                Name = request.Name,
                Location = request.Location,
                PricePerHour = request.PricePerHour,
                FieldOwnerId = ownerId.Value
            };

            await _fieldRepo.AddAsync(field);

            var fieldResponse = field.Adapt<FieldResponseDto>();

            return Result.Success(fieldResponse!);
        }

        public async Task<Result<PaginatedList<FieldResponseDto>>> GetAllAsync(RequestFilter filter,  CancellationToken cancellation = default)
        {
            var query = _fieldRepo.GetAll();

            if(!string.IsNullOrEmpty(filter.SearchValue))
            {
                query = query.Where(x => x.Location.Contains(filter.SearchValue));
            }

            var projected = query
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Location,
                    f.PricePerHour,
                    Rating = f.Reviews
                        .Where(x => !x.IsDeleted)
                        .Select(r => (float?)r.Rating)
                        .Average() ?? 0,
                    Images = f.Images
                        .Where(x => !x.IsDeleted)
                        .Select(x => new GetFieldImages(x.Id, x.ImageURL))
                });

            var sortColumnKey = string.IsNullOrWhiteSpace(filter.SortColumn)
                ? FieldSortingConfig.DefaultColumn
                : filter.SortColumn.ToUpper();

            if (!FieldSortingConfig.ColumnMap.ContainsKey(sortColumnKey))
            {
                sortColumnKey = FieldSortingConfig.DefaultColumn;
            }

            var columnName = FieldSortingConfig.ColumnMap[sortColumnKey];

            var direction = string.IsNullOrWhiteSpace(filter.SortDirection)
                ? columnName == "Rating" ? "DESC" : "ASC"
                : filter.SortDirection.ToUpper();

            if (direction != "ASC" && direction != "DESC")
            {
                direction = "ASC";

                if (columnName == "Rating") direction = "DESC";
            }

            projected = projected.OrderBy($"{columnName} {direction}");

            var source = projected
                .Select(f => new FieldResponseDto(
                    f.Id,
                    f.Name,
                    f.Location,
                    f.PricePerHour,
                    f.Rating,
                    f.Images.ToList()
                ));
            var fields = await PaginatedList<FieldResponseDto>.CreateAsync(source, filter.PageNumber, filter.PageSize);

            return Result.Success(fields);
        }

        public async Task<Result<FieldResponseDto>> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            var field = await _fieldRepo.GetById(id)
                .Select(f => new FieldResponseDto(
                    f.Id,
                    f.Name,
                    f.Location,
                    f.PricePerHour,
                    f.Reviews.Where(x => !x.IsDeleted).Select(r => (float?)r.Rating).Average() ?? 0,
                    f.Images
                        .Where(x => !x.IsDeleted)
                        .Select(x => new GetFieldImages(x.Id, x.ImageURL)).ToList()
                )).FirstOrDefaultAsync(cancellation);

            if (field is null)
                return Result.Failure<FieldResponseDto>(FieldErrors.NotFound);

            return Result.Success(field);
        }

        public async Task<Result<IEnumerable<FieldResponseDto>>> GetByOwnerIdAsync(string ownerId, CancellationToken cancellation = default)
        {
            var ownerIdResult = await GetOwnerIdByUserIdAsync(ownerId, cancellation);

            if (ownerIdResult.IsFailure)
                return Result.Failure<IEnumerable<FieldResponseDto>>(ownerIdResult.Error);

            var fields = await _fieldRepo.GetByOwnerId(ownerIdResult.Value)
                .Select(f => new FieldResponseDto(
                    f.Id,
                    f.Name,
                    f.Location,
                    f.PricePerHour,
                    f.Reviews.Where(x => !x.IsDeleted).Select(r => (float?)r.Rating).Average() ?? 0,
                    f.Images
                        .Where(x => !x.IsDeleted)
                        .Select(x => new GetFieldImages(x.Id, x.ImageURL)).ToList()
                )).ToListAsync(cancellation);


            return Result.Success<IEnumerable<FieldResponseDto>>(fields);
        }      

        public async Task<Result> UpdateAsync(int id, UpdateFieldRequestDto request, string userId, CancellationToken cancellation = default)
        {
            var oldField = await _fieldRepo.GetById(id)
                .FirstOrDefaultAsync(cancellation);

            if (oldField is null)
                return Result.Failure(FieldErrors.NotFound);

            var ownerId = await GetOwnerIdByUserIdAsync(userId, cancellation);

            if (ownerId.IsFailure)
                return Result.Failure<FieldResponseDto>(ownerId.Error);

            if (oldField.FieldOwnerId != ownerId.Value)
                return Result.Failure(FieldErrors.Unauthorized);

            var field = new Field
            {
                Name = request.Name,
                Location = request.Location,
                PricePerHour = request.PricePerHour
            };
            
            await _fieldRepo.UpdateAsync(id, field, cancellation);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, string userId, CancellationToken cancellation = default)
        {
            var field = await _fieldRepo.GetById(id)
                .FirstOrDefaultAsync(cancellation);

            if (field is null)
                return Result.Failure(FieldErrors.NotFound);

            var ownerId = await GetOwnerIdByUserIdAsync(userId, cancellation);

            if (ownerId.IsFailure)
                return Result.Failure<FieldResponseDto>(ownerId.Error);

            if (field.FieldOwnerId != ownerId.Value)
                return Result.Failure(FieldErrors.Unauthorized);

            await _fieldRepo.DeleteAsync(id, cancellation);

            return Result.Success();
        }

        public async Task<Result> UploadImagesAsync(int id, string userId,  IFormFileCollection images, CancellationToken cancellation = default)
        {
            var field = await _fieldRepo.GetById(id)
                .FirstOrDefaultAsync(cancellation);

            if (field is null)
                return Result.Failure(FieldErrors.NotFound);

            var ownerId = await GetOwnerIdByUserIdAsync(userId, cancellation);

            if (ownerId.IsFailure)
                return Result.Failure(ownerId.Error);

            if (field.FieldOwnerId != ownerId.Value)
                return Result.Failure(FieldErrors.Unauthorized);

            var fieldImagesCount = await _fieldRepo.GetFieldImagesCountAsync(id, cancellation);

            if(images.Count() + fieldImagesCount > 3) 
                return Result.Failure(FieldErrors.FieldImagesLimit);

            List<FieldImage> fieldImages = [];

            foreach(var image in images)
            {
                var fildImage = await SaveFieldImages(image, id, cancellation);
                fieldImages.Add(fildImage);
            }

            await _fieldRepo.UploadImageAsync(fieldImages, cancellation);
            return Result.Success();
        }

        public async Task<Result> DeleteImageAsync(int fieldId, string userId, int imageId, CancellationToken cancellation)
        {
            var field = await _fieldRepo.GetById(fieldId)
                .FirstOrDefaultAsync(cancellation);

            if (field is null)
                return Result.Failure(FieldErrors.NotFound);

            var ownerId = await GetOwnerIdByUserIdAsync(userId, cancellation);

            if (ownerId.IsFailure)
                return Result.Failure(ownerId.Error);

            if (field.FieldOwnerId != ownerId.Value)
                return Result.Failure(FieldErrors.Unauthorized);

            var fieldImage = await _fieldRepo.GetImageAsync(imageId, cancellation);

            if (fieldImage is null)
                return Result.Failure(FieldErrors.FieldImagesNotFount);

            if (fieldImage.FieldId != fieldId)
                return Result.Failure(FieldErrors.Unauthorized);

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, fieldImage.ImageURL);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            await _fieldRepo.DeleteImageAsync(fieldId ,imageId, cancellation);

            return Result.Success();
        }

        private async Task<FieldImage> SaveFieldImages(IFormFile image, int fieldId, CancellationToken cancellation = default)
        {
            var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(imagesPath, fileName);

            var root = _webHostEnvironment.WebRootPath;
            _logger.LogInformation("WebRootPath = {path}", root);

            using var stream = new FileStream(path, FileMode.Create);

            await image.CopyToAsync(stream, cancellation);

            return new FieldImage
            {
                ImageURL = $"images/{fileName}",
                FieldId = fieldId
            };
        }
        private async Task<Result<int>> GetOwnerIdByUserIdAsync(string userId, CancellationToken cancellation)
        {
            var owner = await _fieldOwnerRepo.GetOwnerByUserId(userId)
                .FirstOrDefaultAsync(cancellation);

            if (owner == null)
                return Result.Failure<int>(FieldOwnerErrors.NotFound);

            return Result.Success(owner.Id);
        }
    }
}