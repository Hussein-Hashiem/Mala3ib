namespace Mala3ib.DAL.Repo.Implementation
{
    public class FieldOwnerRepo : IFieldOwnerRepo
    {
        private readonly ApplicationDbContext _context;

        public FieldOwnerRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FieldOwner fieldOwner)
        {
            await _context.FieldOwners.AddAsync(fieldOwner);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId, CancellationToken cancellation = default)
        {
            await _context.FieldOwners
                .Where(x => x.UserId == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.IsDeleted, true)
                );

            await _context.Users
                .Where(x => x.Id == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.IsDeleted, true)
                );
        }

        public IQueryable<FieldOwner> Get(string userId)
        {
            var fieldOwner = _context.FieldOwners
                            .Where(x => x.UserId == userId)
                            .AsNoTracking();

            return fieldOwner;
        }

        public async Task<bool> FieleOwnerIsExist(string userId, CancellationToken cancellation = default)
        {
            return await _context.FieldOwners
               .AnyAsync(p => p.UserId == userId && !p.IsDeleted, cancellation);
        }

        public async Task UpdateAsync(string userId, FieldOwner request, CancellationToken cancellation = default)
        {
            await _context.FieldOwners
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.DateOfBirth, request.DateOfBirth)
                    .SetProperty(x => x.Status, request.Status)
                );

            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.FirstName, request.User.FirstName)
                    .SetProperty(x => x.LastName, request.User.LastName)
                    .SetProperty(x => x.PhoneNumber, request.User.PhoneNumber)
                );
        }
        public IQueryable<FieldOwner> GetOwnerByUserId(string userId)
        {
            return _context.FieldOwners.Where(o => o.UserId == userId);
        }

        public IQueryable<FieldOwner> GetAll(Status? status = null)
        {
            var query = _context.FieldOwners
                .Where(x => !x.IsDeleted)
                .AsNoTracking();

            if (status is not null)
                query = query.Where(x => x.Status == status);

            return query;
        }

        public async Task UpdateStatusAsync(string userId, Status status, CancellationToken cancellation = default)
        {
            await _context.FieldOwners
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(x => x.Status, status), cancellation);
        }
    }
}