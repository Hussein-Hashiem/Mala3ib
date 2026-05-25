
namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IAuthService
    {
        #region Login
        Task<Result<AuthResponseDto>?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthResponseDto>?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        #endregion

        #region Regisertation and Comfirmation Email
        Task<Result<RegisterReponseDto>> RegisterPlayerAsync(RegisterPlayerDto request, CancellationToken cancellationToken = default);
        Task<Result<RegisterReponseDto>> RegisterFieldOwnerAsync(RegisterFieldOwnerDto request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request);
        Task<Result<RegisterReponseDto>> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request);
        #endregion

        #region Forget Password
        Task<Result> SendResetPasswordCodeAsync(string email);
        Task<Result> VerifyResetPasswordOtpAsync(string email, string otp);
        Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);
        #endregion
    }
}
