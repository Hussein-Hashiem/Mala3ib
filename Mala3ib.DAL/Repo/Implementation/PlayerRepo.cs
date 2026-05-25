namespace Mala3ib.DAL.Repo.Implementation
{
    public class PlayerRepo : IPlayerRepo
    {
        private readonly ApplicationDbContext _context;

        public PlayerRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);

            await _context.SaveChangesAsync();
        }

        public IQueryable<Player> Get(string userId)
        {
            var player = _context.Players
                .Where(x => x.UserId == userId)
                .AsNoTracking();

            return player;
        }

        public IQueryable<Player> GetAll()
        {
            var player = _context.Players
                .AsNoTracking();

            return player;
        }

        public async Task<bool> IsExistAsync(int Id, CancellationToken cancellation = default)
        {
            return await _context.Players
                .AnyAsync(p => p.Id == Id && !p.IsDeleted, cancellation);
        }

        public async Task UpdateAsync(string userId, Player request, CancellationToken cancellation = default)
        {
            await _context.Players
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.DateOfBirth, request.DateOfBirth)
                );

            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setter =>
                     setter.SetProperty(x => x.FirstName, request.User.FirstName)
                    .SetProperty(x => x.LastName, request.User.LastName)
                    .SetProperty(x => x.PhoneNumber, request.User.PhoneNumber)
                );

            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(string userId, CancellationToken cancellation = default)
        {
            await _context.Players
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
        public async Task<int?> GetPlayerIdByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Players
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .Select(p => p.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
