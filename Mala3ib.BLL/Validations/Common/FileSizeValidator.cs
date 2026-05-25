namespace Mala3ib.BLL.Validations.Common
{
    public class FileSizeValidator : AbstractValidator<IFormFile>
    {
        public FileSizeValidator()
        {
            RuleFor(x => x)
                .Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
                .WithMessage($"Max file size {FileSettings.MaxFileSizeInMB} MB.")
                .When(x => x is not null);
        }
    }
}
