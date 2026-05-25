namespace Mala3ib.BLL.Contracts.Authentication
{
    public record RefreshTokenRequestDto(
        string Token,
        string RefreshToken
    );
}
