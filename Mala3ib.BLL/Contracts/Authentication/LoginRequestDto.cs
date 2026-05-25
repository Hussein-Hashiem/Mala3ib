namespace Mala3ib.BLL.Contracts.Authentication
{
    public record LoginRequestDto (
        string Email, 
        string Password
    );
}
