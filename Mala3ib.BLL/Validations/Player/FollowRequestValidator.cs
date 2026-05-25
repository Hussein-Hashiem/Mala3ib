using Mala3ib.BLL.Contracts.Follow;

namespace Mala3ib.BLL.Validations.Player
{
    public class FollowRequestValidator : AbstractValidator<FollowRequestDto>
    {
        public FollowRequestValidator()
        {
            RuleFor(x => x.TargetUserId)
                .NotEmpty();
        }
    }
}
