using Microsoft.Extensions.Logging;

namespace Mala3ib.BLL.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPlayerRepo _playerRepo;
        private readonly IFieldOwnerRepo _fieldOwnerRepo;
        private readonly ILogger<AuthService> _logger;

        private readonly int _refreshTokenExpiryDays = 14;
        public AuthService(IJwtProvider jwtProvider,
            UserManager<ApplicationUser> userManager,
            IEmailVerificationService emailVerificationService,
            IPlayerRepo playerRepo,
            IFieldOwnerRepo fieldOwnerRepo,
            ILogger<AuthService> logger)
        {
            _jwtProvider = jwtProvider;
            _userManager = userManager;
            _emailVerificationService = emailVerificationService;
            _playerRepo = playerRepo;
            _fieldOwnerRepo = fieldOwnerRepo;
            _logger = logger;
        }
        public async Task<Result<AuthResponseDto>?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidCredentials);

            if(user.IsDeleted)
                return Result.Failure<AuthResponseDto>(UserErrors.NotFouond);

            if (!user.EmailConfirmed)
                return Result.Failure<AuthResponseDto>(UserErrors.EmailNotConfirmed);

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidCredentials);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(DefaultRoles.FieldOwner))
            {
                //var fieldOwnerStatus = _fieldOwnerRepo.GetOwnerByUserId(user.Id)
                //    .Select(x => x.Status)
                //    .FirstOrDefault();

                //if (fieldOwnerStatus == Status.Pending)
                //    return Result.Failure<AuthResponseDto>(UserErrors.FieldOwnerPending);

                //else if (fieldOwnerStatus == Status.Rejected)
                //    return Result.Failure<AuthResponseDto>(UserErrors.FieldOwnerRejected);
            }

            var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            return Result.Success(new AuthResponseDto(user.Id, user.Email, user.FirstName, user.LastName, token, userRoles.FirstOrDefault()!, expiresIn, refreshToken, refreshTokenExpiration));
        }

        public async Task<Result<AuthResponseDto>?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponseDto>(UserErrors.InvalidTokens);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Result.Failure<AuthResponseDto>(UserErrors.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null) return Result.Failure<AuthResponseDto>(UserErrors.InvalidTokens);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var userRoles = await _userManager.GetRolesAsync(user);

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            var result = new AuthResponseDto(user.Id, user.Email, user.FirstName, user.LastName, newToken, userRoles.FirstOrDefault()!, expiresIn, newRefreshToken, refreshTokenExpiration);
            return Result.Success(result);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure(UserErrors.InvalidTokens);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(u => u.Token == refreshToken && u.IsActive);
            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidTokens);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return Result.Success();
        }

        public async Task<Result<RegisterReponseDto>> RegisterPlayerAsync(RegisterPlayerDto request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure<RegisterReponseDto>(UserErrors.DuplicatedEmail);

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var player = new Player
                {
                    UserId = user.Id,
                    DateOfBirth = request.DateOfBirth
                };

                await _playerRepo.AddAsync(player);

                await _userManager.AddToRoleAsync(user, DefaultRoles.Player);

                BackgroundJob.Enqueue<IEmailVerificationService>(x => x.SendEmailVerificationOtpAsync(user));

                return Result.Success(new RegisterReponseDto(user.Id));
            }

            var error = result.Errors.First();
            return Result.Failure<RegisterReponseDto>(new DAL.Abstraction.Error(error.Code, error.Description, ErrorType.BadRequest));
        }

        public async Task<Result<RegisterReponseDto>> RegisterFieldOwnerAsync(RegisterFieldOwnerDto request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure<RegisterReponseDto>(UserErrors.DuplicatedEmail);

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var fieldOwner = new FieldOwner
                {
                    UserId = user.Id,
                    DateOfBirth = request.DateOfBirth,
                    Status = Status.Pending
                };

                await _fieldOwnerRepo.AddAsync(fieldOwner);

                await _userManager.AddToRoleAsync(user, DefaultRoles.FieldOwner);

                BackgroundJob.Enqueue<IEmailVerificationService>(x => x.SendEmailVerificationOtpAsync(user));

                return Result.Success(new RegisterReponseDto(user.Id));
            }

            var error = result.Errors.First();
            return Result.Failure<RegisterReponseDto>(new DAL.Abstraction.Error(error.Code, error.Description, ErrorType.BadRequest));
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            return await _emailVerificationService.VerifyEmailOtpAsync(user, request.Code);
        }

        public async Task<Result<RegisterReponseDto>> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Success(new RegisterReponseDto(""));

            if (user.EmailConfirmed)
                return Result.Failure<RegisterReponseDto>(UserErrors.DuplicatedConfirmation);

            BackgroundJob.Enqueue<IEmailVerificationService>(x => x.SendEmailVerificationOtpAsync(user));

            return Result.Success(new RegisterReponseDto(user.Id));
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Success(); // misleadin user

            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailNotConfirmed);

            BackgroundJob.Enqueue<IEmailVerificationService>(x => x.SendForgetPasswordOtpAsync(user));

            return Result.Success();
        }

        public async Task<Result> VerifyResetPasswordOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var isValidOtp = await _emailVerificationService.VerifyForgetPasswordOtpAsync(user, otp);

            if (isValidOtp.IsFailure)
                return Result.Failure(new DAL.Abstraction.Error("Invalid.Otp", "The verification code is incorrect.", ErrorType.Unauthorized));

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!resetResult.Succeeded)
            {
                var error = resetResult.Errors.First();

                return Result.Failure(new DAL.Abstraction.Error(error.Code, error.Description, ErrorType.BadRequest));
            }

            return Result.Success();
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
