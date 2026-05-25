namespace Mala3ib.BLL.Contracts.Authentication
{
    public record ResetPasswordRequestDto(
        string Email,
        string NewPassword
    );
}
