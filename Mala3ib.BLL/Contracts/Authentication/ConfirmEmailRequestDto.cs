namespace Mala3ib.BLL.Contracts.Authentication
{
    public record ConfirmEmailRequestDto(
        string UserId, 
        string Code
    );
}
