using Mala3ib.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace Mala3ib.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set;} 
        public bool IsDeleted { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
        public List<EmailVerficationOtp> EmailVerficationOtps = [];

        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
    }
}