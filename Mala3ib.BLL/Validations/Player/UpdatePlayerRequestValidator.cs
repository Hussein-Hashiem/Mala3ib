namespace Mala3ib.BLL.Validations.Player
{
    public class UpdatePlayerRequestValidator : AbstractValidator<UpdatePlayerRequestDto>
    {
        public UpdatePlayerRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
        }
    }
}
