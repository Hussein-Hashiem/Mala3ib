namespace Mala3ib.DAL.Repo.Implementation
{
    public class FieldRepo : IFieldRepo
    {
        private readonly ApplicationDbContext _context;

        public FieldRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Field field)
        {
            await _context.Fields.AddAsync(field);

            await _context.SaveChangesAsync();
        }

        public IQueryable<Field> GetAll()
        {
            var fields = _context.Fields.Where(f => !f.IsDeleted)
                .AsNoTracking();

            return fields;
        }

        public IQueryable<Field> GetById(int id)
        {
            var field = _context.Fields
                .Where(x => x.Id == id && !x.IsDeleted)
                .AsNoTracking();

            return field;
        }

        public IQueryable<Field> GetByOwnerId(int fieldOwnerId)
        {
            var field = _context.Fields
                .Where(x => x.FieldOwnerId == fieldOwnerId && !x.IsDeleted)
                .AsNoTracking();

            return field;
        }

        public async Task<decimal> GetPrice(int id, CancellationToken cancellation = default)
        {
            return await _context.Fields
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => x.PricePerHour)
                .FirstOrDefaultAsync(cancellation);
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Fields
                .AnyAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task UpdateAsync(int id, Field request, CancellationToken cancellation = default)
        {
            await _context.Fields
                .Where(f => f.Id == id)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.Name, request.Name)
                    .SetProperty(x => x.Location, request.Location)
                    .SetProperty(x => x.PricePerHour, request.PricePerHour)
                );
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            var affectedRows = await _context.Fields
                .Where(x => x.Id == id && !x.IsDeleted)
                .ExecuteUpdateAsync(setter =>
                setter.SetProperty(x => x.IsDeleted, true)
                );
        }

        public async Task<int> GetFieldImagesCountAsync(int id, CancellationToken cancellation = default)
        {
            return await _context.FieldImages.CountAsync(x => x.FieldId == id && !x.IsDeleted, cancellation);
        }

        public async Task UploadImageAsync(List<FieldImage> fieldImages, CancellationToken cancellation = default)
        {
            await _context.FieldImages.AddRangeAsync(fieldImages, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<FieldImage?> GetImageAsync(int imageId, CancellationToken cancellation = default)
        {
            return await _context.FieldImages.FirstOrDefaultAsync(x => x.Id == imageId && !x.IsDeleted, cancellation);
        }

        public async Task DeleteImageAsync(int fieldId, int imageId, CancellationToken cancellation = default)
        {
            await _context.FieldImages
                .Where(x => x.FieldId == fieldId && x.Id == imageId)
                .ExecuteUpdateAsync(setter =>
                setter.SetProperty(x => x.IsDeleted, true)
                , cancellation);
        }
    }
}