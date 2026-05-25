using Mala3ib.BLL.Contracts.Field;

namespace Mala3ib.BLL.Validations.Field
{
    public class UpdateFieldRequestValidator : AbstractValidator<UpdateFieldRequestDto>
    {
        public UpdateFieldRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Location)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.PricePerHour)
                .GreaterThan(0);
        }
    }
}