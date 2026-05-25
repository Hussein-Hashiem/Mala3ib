namespace Mala3ib.BLL.Service.Implementation
{
    public class FieldSlotService : IFieldSlotService
    {
        private readonly IFieldSlotRepo _fieldSlotRepo;
        private readonly IFieldOwnerRepo _fieldOwnerRepo;
        private readonly IFieldRepo _fieldRepo;
        public FieldSlotService(IFieldSlotRepo fieldSlotRepo, IFieldOwnerRepo fieldOwnerRepo, IFieldRepo fieldRepo)
        {
            _fieldSlotRepo = fieldSlotRepo;
            _fieldOwnerRepo = fieldOwnerRepo;
            _fieldRepo = fieldRepo;
        }
        private async Task<Result> ValidateOwnerAsync(string userId, int fieldId, CancellationToken cancellation)
        {
            var isValid = await _fieldRepo
                    .GetAll() 
                    .AnyAsync(f => f.Id == fieldId && f.FieldOwner.UserId == userId && !f.IsDeleted && !f.FieldOwner.IsDeleted, cancellation);

            if (!isValid)
                return Result.Failure(FieldSlotErrors.Unauthorized);

            return Result.Success();
        }
        public async Task<Result> AddAsync(AddFieldSlotRequestDto request, string userId, int fieldId, CancellationToken cancellation = default)
        {
            var validation = await ValidateOwnerAsync(userId, fieldId, cancellation);
            if (!validation.IsSuccess)
                return validation;

            var isSlotAvailable = await _fieldSlotRepo
                .IsSlotAvailableAsync(fieldId, request.StartDate, request.EndDate, null, cancellation);
            
            if (!isSlotAvailable)
                return Result.Failure(FieldSlotErrors.NotAvialable);

            var fieldPrice = await _fieldRepo.GetPrice(fieldId, cancellation); 

            var fieldSlot = new FieldSlot
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                FieldId = fieldId,
                MaxPlayers = request.MaxPlayers,
                Price = (decimal)(request.EndDate - request.StartDate).TotalHours * fieldPrice,
            };

            await _fieldSlotRepo.AddAsync(fieldSlot);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, int fieldId, string userId, CancellationToken cancellation = default)
        {
            var validation = await ValidateOwnerAsync(userId, fieldId, cancellation);
            if (!validation.IsSuccess)
                return validation;

            var fieldSlot = await _fieldSlotRepo.GetById(id)
                .FirstOrDefaultAsync(cancellation);

            if (fieldSlot is null)
                return Result.Failure(FieldSlotErrors.NotFound);

            if (fieldSlot.FieldId != fieldId)
                return Result.Failure(FieldSlotErrors.NotFound);

            await _fieldSlotRepo.DeleteAsync(id, cancellation);
            return Result.Success();
        }
        public async Task<Result> UpdateAsync(int id, UpdateFieldSlotRequestDto request, string userId, int fieldId, CancellationToken cancellation = default)
        {
            var validation = await ValidateOwnerAsync(userId, fieldId, cancellation);
            if (!validation.IsSuccess)
                return validation;

            var fieldSlot = await _fieldSlotRepo.GetById(id)
                .Select(x => new FieldSlot
                {
                    FieldId = x.FieldId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    MaxPlayers = x.MaxPlayers,
                    Price = x.Field.PricePerHour
                })
                .FirstOrDefaultAsync(cancellation);                

            if (fieldSlot is null)
                return Result.Failure(FieldSlotErrors.NotFound);

            if (fieldSlot.FieldId != fieldId)
                return Result.Failure(FieldSlotErrors.NotFound);

            var isSlotAvailable = await _fieldSlotRepo
                .IsSlotAvailableAsync(fieldId, request.StartDate, request.EndDate, id, cancellation);
            if (!isSlotAvailable)
                return Result.Failure(FieldSlotErrors.NotAvialable);

            // new Values
            fieldSlot.StartDate = request.StartDate;
            fieldSlot.EndDate = request.EndDate;
            fieldSlot.MaxPlayers = request.MaxPlayers;
            fieldSlot.Price = (decimal)(request.EndDate - request.StartDate).TotalHours * fieldSlot.Price;

            await _fieldSlotRepo.UpdateAsync(id, fieldSlot, cancellation);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<FieldSlotResponseDto>>> GetAvailableSlots(int fieldId, DateTime? date, CancellationToken cancellation = default)
        {
            var actualDate = date ?? DateTime.Today;

            var fieldSlots = await _fieldSlotRepo.GetAvailableSlots(fieldId, actualDate)
                .Select(f => new FieldSlotResponseDto(
                    f.Id,
                    f.StartDate,
                    f.EndDate,
                    f.Price,
                    f.MaxPlayers,
                    f.Players.Count(),  // Will be changed after implement Invitaion Table
                    f.IsBooked
                )).ToListAsync(cancellation);

            return Result.Success<IEnumerable<FieldSlotResponseDto>>(fieldSlots);
        }

        public async Task<Result<IEnumerable<FieldSlotResponseDto>>> GetByFieldIdAsync(int fieldId, CancellationToken cancellation = default)
        {
            var fieldSlots = await _fieldSlotRepo.GetByFieldId(fieldId)
                .Select(f => new FieldSlotResponseDto(
                    f.Id,
                    f.StartDate,
                    f.EndDate,
                    f.Price,
                    f.MaxPlayers,
                    f.Players.Count(),  // Will be changed after implement Invitaion Table
                    f.IsBooked
                )).ToListAsync(cancellation);

            return Result.Success<IEnumerable<FieldSlotResponseDto>>(fieldSlots);
        }

        public async Task<Result<FieldSlotResponseDto>> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            var fieldSlot = await _fieldSlotRepo.GetById(id)
                .Select(f => new FieldSlotResponseDto(
                    f.Id,
                    f.StartDate,
                    f.EndDate,
                    f.Price,
                    f.MaxPlayers,
                    f.Players.Count(),  // Will be changed after implement Invitaion Table
                    f.IsBooked
                )).FirstOrDefaultAsync(cancellation);

            if (fieldSlot is null)
                return Result.Failure<FieldSlotResponseDto>(FieldSlotErrors.NotFound);

            return Result.Success(fieldSlot);
        }

    }
}