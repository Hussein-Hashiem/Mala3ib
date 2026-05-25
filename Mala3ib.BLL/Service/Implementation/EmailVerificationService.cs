namespace Mala3ib.BLL.Service.Implementation
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;
        private readonly EmailBodyBuilder _emailBodyBuilder;
        public EmailVerificationService(ApplicationDbContext context,
                IEmailSender emailService,
                EmailBodyBuilder emailBodyBuilder)
        {
            _context = context;
            _emailService = emailService;
            _emailBodyBuilder = emailBodyBuilder;
        }

        public async Task SendEmailVerificationOtpAsync(ApplicationUser user)
        {
            await SendOtpAsync(user, OtpType.EmailVerification, "EmailConfirmation.html");
        }

        public async Task SendForgetPasswordOtpAsync(ApplicationUser user)
        {
            await SendOtpAsync(user, OtpType.PasswordReset, "ForgetPassword.html");
        }

        private async Task SendOtpAsync(ApplicationUser user, OtpType type, string template)
        {
            var otp = GenerateOtp();

            var userOtp = new EmailVerficationOtp
            {
                UserId = user.Id,
                Code = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10),
                Type = type,
                IsUsed = false
            };

            await SendEmail(user, otp, template);

            await _context.EmailVerficationOtps.AddAsync(userOtp);
            await _context.SaveChangesAsync();
        }

        public async Task<Result> VerifyEmailOtpAsync(ApplicationUser user, string code)
        {
            var result = await VerifyOtpAsync(user, code, OtpType.EmailVerification);

            if (!result.IsSuccess)
                return result;

            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> VerifyForgetPasswordOtpAsync(ApplicationUser user, string code)
        {
            return await VerifyOtpAsync(user, code, OtpType.PasswordReset);
        }

        private async Task<Result> VerifyOtpAsync(ApplicationUser user, string code, OtpType type)
        {
            var userOtp = await _context.EmailVerficationOtps
                .Where(x => x.UserId == user.Id && x.Type == type)
                .OrderByDescending(x => x.ExpirationTime)
                .FirstOrDefaultAsync();

            if (userOtp is null)
                return Result.Failure(new Error("Invalid.Otp", "Invalid verification code", ErrorType.Unauthorized));

            if (userOtp.IsUsed)
                return Result.Failure(new Error("Invalid.Otp", "This verification code has already been used.", ErrorType.Unauthorized));

            if (userOtp.ExpirationTime < DateTime.UtcNow)
                return Result.Failure(new Error("Invalid.Otp", "The verification code has expired.", ErrorType.Unauthorized));

            if (userOtp.Code != code)
                return Result.Failure(new Error("Invalid.Otp", "The verification code is incorrect.", ErrorType.Unauthorized));

            userOtp.IsUsed = true;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        
        private string GenerateOtp()
        {
            var random = new Random();

            return random.Next(100000, 999999).ToString();
        }

        private async Task SendEmail(ApplicationUser user, string otp, string templateName)
        {
            var emailBody = _emailBodyBuilder.GenerateEmailBody(
                templateName,
                new Dictionary<string, string>
                {
                    { "{{UserName}}", $"{user.FirstName} {user.LastName}" },
                    { "{{OTP_CODE}}", otp }
                });

            await _emailService.SendEmailAsync(user.Email!, "✅ Mala3ib Verification Code", emailBody);
        }
    }
}
