
using Mala3ib.BLL.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Mala3ib.BLL.Service.Implementation
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepo _playerRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;

        public PlayerService(IPlayerRepo playerRepo,
            UserManager<ApplicationUser> userManager,
            ICacheService cacheService)
        {
            _playerRepo = playerRepo;
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<Result<PlayerProfileDto>> GetAsync(string currentUserId, string userId, CancellationToken cancellation = default)
        {
            var cacheProfile = await _cacheService.GetAsync<PlayerProfileDto>(CacheKeys.ProfileKey(userId), cancellation);

            if(cacheProfile is not null)
            {
                return Result.Success(cacheProfile);
            }
            
            var player = await _playerRepo.Get(userId)
                .Select(p => new PlayerProfileDto(
                    p.User.Id,
                    p.User.Email!,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber!,
                    p.User.Image,
                    p.DateOfBirth,
                    p.User.Followers.Count(x => !x.IsDeleted),
                    p.User.Following.Count(x => !x.IsDeleted),
                    currentUserId == userId ? false : p.User.Followers.Any(x => x.FollowerId == currentUserId && !x.IsDeleted)
                ))
                .FirstOrDefaultAsync(cancellation);

            if (player is null)
                return Result.Failure<PlayerProfileDto>(PlayerErrors.NotFound);

            await _cacheService.SetAsync<PlayerProfileDto>(CacheKeys.ProfileKey(userId), player, cancellation);

            return Result.Success(player);
        }

        public async Task<Result> UpdateAsync(string userId, UpdatePlayerRequestDto request, CancellationToken cancellation = default)
        {
            var oldPlayer = await _playerRepo.Get(userId).FirstOrDefaultAsync(cancellation);

            if (oldPlayer == null)
                return Result.Failure(PlayerErrors.NotFound);

            var player = new Player
            {
                DateOfBirth = request.DateOfBirth,
                User = new ApplicationUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                }
            };

            await _cacheService.RemoveAsync(CacheKeys.ProfileKey(userId), cancellation);
            
            await _playerRepo.UpdateAsync(userId, player, cancellation);

            return Result.Success(player);
        }

        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
        }

        public async Task<Result> DeleteAsync(string userId, CancellationToken cancellation = default)
        {
            var oldPlayer = _playerRepo.Get(userId).FirstOrDefault();

            if (oldPlayer == null)
                return Result.Failure(PlayerErrors.NotFound);

            await _playerRepo.DeleteAsync(userId, cancellation);

            return Result.Success();
        }

        public async Task<PaginatedList<PlayerInfoDto>> GetAllAsync(RequestFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _playerRepo.GetAll();

            if (!string.IsNullOrEmpty(filter.SearchValue))
            {
                query = query.Where(x =>
                    (x.User.FirstName + " " + x.User.LastName).Contains(filter.SearchValue) ||
                    x.User.Email!.Contains(filter.SearchValue));
            }

            query = query.OrderBy(x => x.User.Id);

            var source = query.Select(x => new PlayerInfoDto(
                x.User.Id,
                x.User.FirstName + " " + x.User.LastName,
                x.User.Email!,
                x.User.Image
            ));

            var players = await PaginatedList<PlayerInfoDto>.CreateAsync(source, filter.PageNumber, filter.PageSize, cancellationToken);

            return players;
        }

    }
}
