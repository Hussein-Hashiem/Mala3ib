namespace Mala3ib.BLL.Validations.Authentication
{
    public class VerifyResetPasswordOtpRequestValidator : AbstractValidator<VerifyResetPasswordOtpRequestDto>
    {
        public VerifyResetPasswordOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Otp)
                .NotEmpty();
        }
    }
}
