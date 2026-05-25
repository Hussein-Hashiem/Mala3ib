namespace Mala3ib.BLL.Validations.FieldOwner
{
    public class UpdateFieldOwnerRequestValidator : AbstractValidator<UpdateFieldOwnerRequestDto>
    {
        public UpdateFieldOwnerRequestValidator()
        {
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