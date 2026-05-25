namespace Mala3ib.BLL.Validations.Common
{
    public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageRequestValidator()
        {
            RuleFor(x => x.Image)
                .SetValidator(new FileSizeValidator());

            RuleFor(x => x.Image)
                .SetValidator(new FileContentValidator());

            RuleFor(x => x.Image)
                .Must((request, context) =>
                {
                    var extension = Path.GetExtension(request.Image.FileName.ToLower());
                    return FileSettings.allowedImageExtensions.Contains(extension);
                }).WithMessage("File extension is not allowed")
                .When(x => x.Image is not null);
        }
    }
}
