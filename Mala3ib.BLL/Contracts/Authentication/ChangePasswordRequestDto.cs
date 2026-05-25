namespace Mala3ib.BLL.Contracts.Authentication
{
    public record ChangePasswordRequestDto (
         string CurrentPassword,
         string NewPassword
    );
}
