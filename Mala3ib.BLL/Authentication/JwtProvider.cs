using Mala3ib.DAL.Entities;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Mala3ib.BLL.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> option)
        {
            _jwtOptions = option.Value;
        }

        public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            Claim[] claims = [
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email!),
                new (JwtRegisteredClaimNames.GivenName, user.FirstName),
                new (JwtRegisteredClaimNames.FamilyName, user.LastName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray)
            ];

            var symmeticSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var signingCredentials = new SigningCredentials(symmeticSecurityKey, SecurityAlgorithms.HmacSha256);

            var expiresIn = _jwtOptions.ExpiryMinutes;
            var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);

            var token = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: expirationDate,
                    signingCredentials: signingCredentials
            );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmeticSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmeticSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
