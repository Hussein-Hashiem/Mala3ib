namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IUserRepo
    {
        Task<bool> IsExistAsync(string userId, CancellationToken cancellation = default);
    }
}
