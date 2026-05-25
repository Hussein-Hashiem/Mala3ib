namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IFollowRepo
    {
        Task<bool> FollowAsync(string myPlayerId, string targetPlayerId, CancellationToken cancellation = default);
        Task<bool> UnFollowAsync(string myUserId, string targetUserId, CancellationToken cancellation = default);
        public IQueryable<ApplicationUser> GetFollowingAsync(string userId);
        IQueryable<ApplicationUser> GetFollowersAsync(string userId);
    }
}
