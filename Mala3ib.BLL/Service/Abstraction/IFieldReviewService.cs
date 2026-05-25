using Mala3ib.BLL.Contracts.Common;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFieldReviewService
    {
        Task<Result> AddReviewAsync(string userId, int fieldId, AddReviewRequestDto request, CancellationToken cancellationToken = default);
        Task<Result> UpdateReviewAsync(string userId, int reviewId, UpdateFieldReviewDto request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string userId, int reviewId, bool isAdmin, CancellationToken cancellationToken = default);
        Task<Result<GetFieldReviewDto>> GetReviewAsync(int reviewId, CancellationToken cancellationToken = default);
        Task<Result<PaginatedList<GetFieldReviewDto>>> GetAllReviewsForFieldAsync(int fieldId, RequestFilter filter, CancellationToken cancellationToken = default);
    }
}
