namespace Mala3ib.BLL.Contracts.Authentication
{
    public record AuthResponseDto(
        string Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        string Role,
        int ExpiresIn,
        string RefreshToken,   
        DateTime RefreshTokenExpiration 
    );
}
