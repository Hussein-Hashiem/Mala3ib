namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IEmailVerificationService
    {
        Task SendEmailVerificationOtpAsync(ApplicationUser user);
        Task SendForgetPasswordOtpAsync(ApplicationUser user);

        Task<Result> VerifyEmailOtpAsync(ApplicationUser user, string code);
        Task<Result> VerifyForgetPasswordOtpAsync(ApplicationUser user, string code);
    }
}
