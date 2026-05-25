using Mala3ib.BLL.Contracts.Player;

namespace Mala3ib.BLL.Validations.Player
{
    public class RegisterPlayerValidator : AbstractValidator<RegisterPlayerDto>
    {
        public RegisterPlayerValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, Uppercase, NonAlphanumeric");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                 .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth must be earlier than today.");
        }
    }
}
