namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IPlayerRepo
    {
        Task AddAsync(Player player);
        IQueryable<Player> Get(string userId);
        IQueryable<Player> GetAll();
        Task<bool> IsExistAsync(int Id, CancellationToken cancellation = default);
        Task DeleteAsync(string userId, CancellationToken cancellation = default);
        Task UpdateAsync(string userId, Player request, CancellationToken cancellation = default);
        Task<int?> GetPlayerIdByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
