using Mala3ib.BLL.Contracts.FieldSlot;

namespace Mala3ib.BLL.Validations.FieldSlot
{
    public class AddFieldSlotRequestValidator : AbstractValidator<AddFieldSlotRequestDto>
    {
        public AddFieldSlotRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .WithMessage("Start date must be in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .WithMessage("End date must be in the future");

            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("End date must be greater than start date");

            RuleFor(x => x.MaxPlayers)
                .GreaterThan(0);
        }
    }
}