namespace Mala3ib.BLL.Validations.Authentication
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequestDto>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }
}
