using Mala3ib.DAL.Enums;

namespace Mala3ib.DAL.Entities
{
    public class EmailVerficationOtp
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public DateTime ExpirationTime { get; set; }
        public string UserId { get; set; } = string.Empty;
        public OtpType Type { get; set; } // EmailVerification || PasswordReset
        public ApplicationUser User { get; set; } = default!;
    }
}
