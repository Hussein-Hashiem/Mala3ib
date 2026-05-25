namespace Mala3ib.BLL.Validations.Authentication
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequestDto>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
