namespace Mala3ib.DAL.Repo.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistAsync(string userId, CancellationToken cancellation = default)
        {
            return await _context.Users
                .AnyAsync(x => x.Id == userId && !x.IsDeleted, cancellation);
        }
    }
}
