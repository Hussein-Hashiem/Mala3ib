namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IPlayerService
    {
        Task<Result<PlayerProfileDto>> GetAsync(string currentUserId, string userId, CancellationToken cancellation = default);
        Task<Result> UpdateAsync(string userId, UpdatePlayerRequestDto request, CancellationToken cancellation = default);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequestDto request);
        Task<Result> DeleteAsync(string userId, CancellationToken cancellation = default);
        Task<PaginatedList<PlayerInfoDto>> GetAllAsync(RequestFilter filter, CancellationToken cancellationToken = default);
    }
}
