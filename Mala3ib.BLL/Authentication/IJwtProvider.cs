using Mala3ib.DAL.Entities;

namespace Mala3ib.BLL.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
        string? ValidateToken(string token);
    }
}
