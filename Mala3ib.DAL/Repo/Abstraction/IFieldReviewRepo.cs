namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IFieldReviewRepo
    {
        Task AddAsync(FieldReview fieldReview, CancellationToken cancellationToken);
        Task<bool> IsExistsAsync(int fieldId, int playerId, CancellationToken cancellationToken = default);
        Task<bool> HasDeletedReview(int fieldId, int playerId, CancellationToken cancellationToken = default);
        Task<FieldReview?> GetByIdAsync(int reviewId, CancellationToken cancellationToken = default);
        Task UpdateAsync(FieldReview fieldReview, CancellationToken cancelToken);
        Task DeleteAsync(int reviewId, CancellationToken cancellationToken = default);
        IQueryable<FieldReview> GetReview(int reviewId);
        IQueryable<FieldReview> GetAllReviewsForField(int fieldId);
    }
}
