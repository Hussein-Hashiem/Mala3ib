using Mala3ib.BLL.Abstraction;
using Mala3ib.BLL.Contracts.Follow;

namespace Mala3ib.BLL.Service.Implementation
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepo _followRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICacheService _cacheService;

        public FollowService(IFollowRepo followRepo, IUserRepo userRepo, ICacheService cacheService)
        {
            _followRepo = followRepo;
            _userRepo = userRepo;
            _cacheService = cacheService;
        }

        public async Task<Result> FollowAsync(string currentUserId, FollowRequestDto request, CancellationToken cancellation = default)
        {
            var targetUserIsExist = await _userRepo.IsExistAsync(request.TargetUserId, cancellation);
            if (!targetUserIsExist)
                return Result.Failure(UserErrors.NotFouond);

            if (currentUserId == request.TargetUserId)
                return Result.Failure(FollowErrors.CannotFollowYourself);

            var result =  await _followRepo.FollowAsync(currentUserId, request.TargetUserId, cancellation);

            if(!result)
                return Result.Failure(FollowErrors.AlreadyFollowing);

            await _cacheService.RemoveAsync(CacheKeys.ProfileKey(currentUserId), cancellation);
            await _cacheService.RemoveAsync(CacheKeys.ProfileKey(request.TargetUserId), cancellation);

            return Result.Success();
        }

        public async Task<Result> UnFollowAsync(string currentUserId, FollowRequestDto request, CancellationToken cancellation = default)
        {
            var targetUserIsExist = await _userRepo.IsExistAsync(request.TargetUserId, cancellation);
            if (!targetUserIsExist)
                return Result.Failure(UserErrors.NotFouond);

            if (currentUserId == request.TargetUserId)
                return Result.Failure(FollowErrors.CannotFollowYourself);

            var result = await _followRepo.UnFollowAsync(currentUserId, request.TargetUserId, cancellation);

            if(!result)
                return Result.Failure(FollowErrors.AlreadyUnfollowed);

            await _cacheService.RemoveAsync(CacheKeys.ProfileKey(currentUserId), cancellation);
            await _cacheService.RemoveAsync(CacheKeys.ProfileKey(request.TargetUserId), cancellation);

            return Result.Success();
        }

        public async Task<Result<List<FollowUserDto>>> GetFollowingAsync(string userId, CancellationToken cancellation = default)
        {
            var isExistUser = await _userRepo.IsExistAsync(userId, cancellation);

            if (!isExistUser)
                return Result.Failure<List<FollowUserDto>>(UserErrors.NotFouond);

            var followingResult =  _followRepo.GetFollowingAsync(userId);

            var following = await followingResult
                .Select(x => new FollowUserDto
                (
                    x.Id,
                    $"{x.FirstName} {x.LastName}",
                    x.Image
                ))
                .ToListAsync(cancellation);

            return Result.Success(following);
        }

        public async Task<Result<List<FollowUserDto>>> GetFollowersAsync(string userId, CancellationToken cancellation = default)
        {
            var isExistUser = await _userRepo.IsExistAsync(userId, cancellation);

            if(!isExistUser)
                return Result.Failure<List<FollowUserDto>>(UserErrors.NotFouond);

            var followersResult = _followRepo.GetFollowersAsync(userId);
        
            var followers = await followersResult
                .Select(x => new FollowUserDto
                (
                    x.Id,
                    $"{x.FirstName} {x.LastName}",
                    x.Image
                ))
                .ToListAsync(cancellation);

            return Result.Success(followers);
        }
    }
}
