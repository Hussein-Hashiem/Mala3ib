namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFieldOwnerService
    {
        Task<Result<FieldOwnerProfileDto>> GetAsync(string userId, CancellationToken cancellation = default);
        Task<Result> UpdateAsync(string userId, UpdateFieldOwnerRequestDto request, CancellationToken cancellation = default);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequestDto request);
        Task<Result> DeleteAsync(string userId, CancellationToken cancellation = default);
        Task<Result<FieldOwnerDashboardDto>> GetDashboardAsync(string userId, CancellationToken cancellation = default);
        
    }
}