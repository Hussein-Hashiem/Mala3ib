namespace Mala3ib.BLL.Validations.Authentication
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, Uppercase, NonAlphanumeric");
        
        }
    }
}
