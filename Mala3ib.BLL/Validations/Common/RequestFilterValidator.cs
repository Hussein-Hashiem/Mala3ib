using Mala3ib.BLL.Contracts.Common;

namespace Mala3ib.BLL.Validations.Common
{
    public class RequestFilterValidator : AbstractValidator<RequestFilter>
    {
        public RequestFilterValidator()
        {
            RuleFor(x => x.PageNumber)
            .GreaterThan(0);
        }
    }
}
