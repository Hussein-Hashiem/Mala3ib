namespace Mala3ib.BLL.Contracts.Player
{
    public record PlayerInfoDto (
        string UserId, 
        string FullName, 
        string Email,
        string? Image
    );
}
