
using Mala3ib.DAL.Entities;

namespace Mala3ib.DAL.Repo.Implementation
{
    public class FieldReviewRepo : IFieldReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public FieldReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FieldReview fieldReview, CancellationToken cancellationToken)
        {
            await _context.FieldReviews.AddAsync(fieldReview, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsExistsAsync(int fieldId, int playerId, CancellationToken cancellationToken = default)
        {
            return await _context.FieldReviews
               .AnyAsync(x => x.FieldId == fieldId && x.PlayerId == playerId && !x.IsDeleted, cancellationToken);
        }

        public async Task<bool> HasDeletedReview(int fieldId, int playerId, CancellationToken cancellationToken = default)
        {
            return await _context.FieldReviews
               .AnyAsync(x => x.FieldId == fieldId && x.PlayerId == playerId && x.IsDeleted, cancellationToken);
        }

        public  async Task<FieldReview?> GetByIdAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            return  await _context.FieldReviews
                .FirstOrDefaultAsync(x => x.Id == reviewId && !x.IsDeleted, cancellationToken);
        }

        public async Task UpdateAsync(FieldReview fieldReview, CancellationToken cancelToken)
        {
            await _context.FieldReviews
                .Where(x => x.FieldId == fieldReview.FieldId && x.PlayerId == fieldReview.PlayerId)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(f => f.Rating, fieldReview.Rating)
                          .SetProperty(f => f.Comment, fieldReview.Comment)
                          .SetProperty(f => f.IsDeleted, false)
                          .SetProperty(f => f.DateTime, fieldReview.DateTime));
        }      

        public async Task DeleteAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            await _context.FieldReviews
                .Where(x => x.Id == reviewId)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(r => r.IsDeleted, true)
                );
        }
        public IQueryable<FieldReview> GetReview(int reviewId)
        {
            return _context.FieldReviews
                .Where(x => x.Id == reviewId && !x.IsDeleted)
                .AsNoTracking();
        }
        public IQueryable<FieldReview> GetAllReviewsForField(int fieldId)
        {
            return _context.FieldReviews
                .Where(x => x.FieldId == fieldId && !x.IsDeleted)
                .OrderByDescending(x => x.DateTime)
                .AsNoTracking();
        }
    }
}
