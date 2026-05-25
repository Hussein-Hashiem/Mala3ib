namespace Mala3ib.BLL.Validations.Authentication
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
