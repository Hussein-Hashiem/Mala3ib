using Mala3ib.BLL.Contracts.Invitation;

namespace Mala3ib.BLL.Validations.Invitation
{
    public class SendInviationValidator : AbstractValidator<SendInviationDto>
    {
        public SendInviationValidator()
        {
            RuleFor(x => x.TargetPlayerrId)
                .GreaterThan(0)
                .WithMessage("Target player id must be greater than 0");

            RuleFor(x => x.FieldSlotId)
                .GreaterThan(0)
                .WithMessage("Field slot id must be greater than 0");
        }
    }
}