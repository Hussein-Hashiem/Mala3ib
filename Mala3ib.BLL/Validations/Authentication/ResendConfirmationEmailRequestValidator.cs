using System;
using System.Collections.Generic;
using System.Text;

namespace Mala3ib.BLL.Validations.Authentication
{
    internal class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequestDto>
    {
        public ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
