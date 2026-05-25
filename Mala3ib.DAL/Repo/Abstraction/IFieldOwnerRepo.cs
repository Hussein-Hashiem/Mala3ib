namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IFieldOwnerRepo
    {
        Task AddAsync(FieldOwner fieldOwner);
        IQueryable<FieldOwner> Get(string userId);
        Task DeleteAsync(string userId, CancellationToken cancellation = default);
        Task UpdateAsync(string userId, FieldOwner request, CancellationToken cancellation = default);
        IQueryable<FieldOwner> GetOwnerByUserId(string userId);
        IQueryable<FieldOwner> GetAll(Status? status = null);
        Task UpdateStatusAsync(string userId, Status status, CancellationToken cancellation = default);
        Task<bool> FieleOwnerIsExist(string userId, CancellationToken cancellation = default);
    }
}
