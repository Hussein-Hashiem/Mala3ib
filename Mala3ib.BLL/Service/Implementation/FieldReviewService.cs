using Mala3ib.BLL.Contracts.Common;

namespace Mala3ib.BLL.Service.Implementation
{
    public class FieldReviewService : IFieldReviewService
    {
        private readonly IFieldReviewRepo _fieldReviewRepo;
        private readonly IPlayerRepo _playerRepo;
        private readonly IFieldRepo _fieldRepo;

        public FieldReviewService(IFieldReviewRepo fieldReviewRepo, IPlayerRepo playerRepo, IFieldRepo fieldRepo)
        {
            _fieldReviewRepo = fieldReviewRepo;
            _playerRepo = playerRepo;
            _fieldRepo = fieldRepo;
        }

        public async Task<Result> AddReviewAsync(string userId, int fieldId, AddReviewRequestDto request, CancellationToken cancellationToken = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(userId, cancellationToken);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure(PlayerErrors.NotFound);

            var fieldExists = await _fieldRepo.IsExistAsync(fieldId, cancellationToken);

            if (!fieldExists)
                return Result.Failure(FieldErrors.NotFound);

            var alreadyReviewed = await _fieldReviewRepo.IsExistsAsync(fieldId, (int)playerId , cancellationToken);

            if (alreadyReviewed)
                return Result.Failure(FieldReviewErrors.HasReview);

            var review = request.Adapt<FieldReview>();
            review!.PlayerId = (int)playerId;
            review!.FieldId = fieldId;
            review!.DateTime = DateTime.UtcNow;

            var hasDeletedReview = await _fieldReviewRepo.HasDeletedReview(fieldId, (int)playerId, cancellationToken);

            if (hasDeletedReview)
            {
                await _fieldReviewRepo.UpdateAsync(review, cancellationToken);
                return Result.Success();
            }

            await _fieldReviewRepo.AddAsync(review, cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(string userId, int reviewId, bool isAdmin, CancellationToken cancellationToken = default)
        {
            var fieldReview = await _fieldReviewRepo.GetByIdAsync(reviewId, cancellationToken);

            if(fieldReview is null)
                return Result.Failure(FieldReviewErrors.NotFound);

            if(isAdmin)
            {
                await _fieldReviewRepo.DeleteAsync(reviewId, cancellationToken);
                return Result.Success();
            }

            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(userId, cancellationToken);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure(PlayerErrors.NotFound);

            if (fieldReview.PlayerId != playerId)
                return Result.Failure(FieldReviewErrors.NotFound);

            await _fieldReviewRepo.DeleteAsync(reviewId, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<PaginatedList<GetFieldReviewDto>>> GetAllReviewsForFieldAsync(int fieldId, RequestFilter filter, CancellationToken cancellationToken = default)
        {
            var fieldIsExist = await _fieldRepo.IsExistAsync(fieldId, cancellationToken);

            if(!fieldIsExist)
                return Result.Failure<PaginatedList<GetFieldReviewDto>>(FieldErrors.NotFound);

            var query =  _fieldReviewRepo.GetAllReviewsForField(fieldId)
                .Select(x => new GetFieldReviewDto
                (
                    x.Id,
                    x.Comment,
                    x.Rating,
                    x.Player.User.Id,
                    $"{x.Player.User.FirstName} {x.Player.User.LastName}",
                    x.Player.User.Email!
                ));

            var reviews = await PaginatedList<GetFieldReviewDto>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellationToken);

            return Result.Success(reviews);
        }

        public async Task<Result<GetFieldReviewDto>> GetReviewAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var review = await _fieldReviewRepo.GetReview(reviewId)
                .Select(x => new GetFieldReviewDto
                (
                    x.Id,
                    x.Comment,
                    x.Rating,
                    x.Player.User.Id,
                    $"{x.Player.User.FirstName} {x.Player.User.LastName}",
                    x.Player.User.Email!
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (review is null)
                return Result.Failure<GetFieldReviewDto>(FieldErrors.NotFound);


            return Result.Success(review!);
        }

        public async Task<Result> UpdateReviewAsync(string userId, int reviewId, UpdateFieldReviewDto request, CancellationToken cancellationToken = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(userId, cancellationToken);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure(PlayerErrors.NotFound);

            
            var fieldReview = await _fieldReviewRepo.GetByIdAsync(reviewId, cancellationToken);

            if (fieldReview is null)
                return Result.Failure(FieldReviewErrors.NotFound);


            if (fieldReview.PlayerId != playerId)
                return Result.Failure(FieldReviewErrors.NotFound);

            fieldReview.Comment = request.Comment;
            fieldReview.Rating = request.Rating;
            fieldReview!.DateTime = DateTime.UtcNow;

            await _fieldReviewRepo.UpdateAsync(fieldReview, cancellationToken);

            return Result.Success();
        }
    }
}
