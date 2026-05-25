namespace Mala3ib.DAL.Repo.Implementation
{
    public class FollowRepo : IFollowRepo
    {
        private readonly ApplicationDbContext _context;

        public FollowRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> FollowAsync(string myUserId, string targetUserId, CancellationToken cancellation)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(x => x.FollowerId == myUserId && x.FollowingId == targetUserId, cancellation);

            if (follow is not null)
            {
                if (!follow.IsDeleted)
                   return false; // AlreadyFollowed

                // Restore soft deleted
                follow.IsDeleted = false;
                follow.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellation);
                return true;
            }

            var newFollow = new Follow
            {
                FollowerId = myUserId,
                FollowingId = targetUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Follows.AddAsync(newFollow);
            await _context.SaveChangesAsync(cancellation);

            return true;
        }

        public async Task<bool> UnFollowAsync(string myUserId, string targetUserId, CancellationToken cancellation)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(x => x.FollowerId == myUserId && x.FollowingId == targetUserId, cancellation);

            if (follow is not null)
            {
                if (follow.IsDeleted)
                    return false;

                follow.IsDeleted = true;

                await _context.SaveChangesAsync(cancellation);
                return true;
            }

            return false; 
        }

        public IQueryable<ApplicationUser> GetFollowingAsync(string userId)
        {
            return _context.Follows
                .Where(x => x.FollowerId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Following)
                .AsNoTracking();
        }

        public IQueryable<ApplicationUser> GetFollowersAsync(string userId)
        {
            return _context.Follows
                .Where(x => x.FollowingId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Follower)
                .AsNoTracking();
        }
    }
}
