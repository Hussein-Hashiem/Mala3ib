namespace Mala3ib.BLL.Validations.Authentication
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
            .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, Uppercase, NonAlphanumeric")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password should not be equal old password");
        }
    }
}
