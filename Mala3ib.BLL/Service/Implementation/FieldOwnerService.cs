
namespace Mala3ib.BLL.Service.Implementation
{
    public class FieldOwnerService : IFieldOwnerService
    {
        private readonly IFieldOwnerRepo _fieldOwnerRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public FieldOwnerService(IFieldOwnerRepo fieldOwnerRepo, UserManager<ApplicationUser> userManager)
        {
            _fieldOwnerRepo = fieldOwnerRepo;
            _userManager = userManager;
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
            var fieldOwnerIsExist = await _fieldOwnerRepo.FieleOwnerIsExist(userId, cancellation);

            if (!fieldOwnerIsExist)
                return Result.Failure(FieldOwnerErrors.NotFound);

            await _fieldOwnerRepo.DeleteAsync(userId, cancellation);

            return Result.Success();
        }

        public async Task<Result<FieldOwnerProfileDto>> GetAsync(string userId, CancellationToken cancellation = default)
        {
            var fieldOwner = await _fieldOwnerRepo.Get(userId)
                .Select(p => new FieldOwnerProfileDto(
                    p.User.Email!,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber!,
                    p.DateOfBirth,
                    p.User.Image
                ))
                .FirstOrDefaultAsync(cancellation);

            if (fieldOwner is null)
                return Result.Failure<FieldOwnerProfileDto>(FieldOwnerErrors.NotFound);

            return Result.Success(fieldOwner);
        }
        
        public async Task<Result> UpdateAsync(string userId, UpdateFieldOwnerRequestDto request, CancellationToken cancellation = default)
        {
            var fieldOwnerIsExist = await _fieldOwnerRepo.FieleOwnerIsExist(userId, cancellation);

            if(!fieldOwnerIsExist)
                return Result.Failure(FieldOwnerErrors.NotFound);

            var fieldOwner = new FieldOwner
            {
                DateOfBirth = request.DateOfBirth,
                User = new ApplicationUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                }
            };
            await _fieldOwnerRepo.UpdateAsync(userId, fieldOwner, cancellation);
            return Result.Success();
        }

        public async Task<Result<FieldOwnerDashboardDto>> GetDashboardAsync(string userId, CancellationToken cancellation = default)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var result = await _fieldOwnerRepo.Get(userId)
                .Select(fo => new FieldOwnerDashboardDto
                (
                    fo.Fields.Count(f => !f.IsDeleted),
                    fo.Fields
                        .SelectMany(f => f.Slots)
                        .Count(s =>
                            s.StartDate < tomorrow &&
                            s.EndDate >= today &&
                            s.IsBooked
                        ),
                    fo.Fields
                        .SelectMany(f => f.Slots)
                        .Count(s =>
                            s.StartDate < tomorrow &&
                            s.EndDate >= today &&
                            !s.IsBooked
                        )
                ))
                .FirstOrDefaultAsync();

            if(result is null)
                return Result.Failure<FieldOwnerDashboardDto>(FieldOwnerErrors.NotFound);

            return Result.Success(result);
        }
    }
}