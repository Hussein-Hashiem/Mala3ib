namespace Mala3ib.BLL.Contracts.Authentication
{
    public record VerifyResetPasswordOtpRequestDto(
        string Email, 
        string Otp
    );
}
