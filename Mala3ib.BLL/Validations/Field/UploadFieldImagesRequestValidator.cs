
namespace Mala3ib.BLL.Validations.Field
{
    public class UploadFieldImagesRequestValidator : AbstractValidator<UploadFieldImagesRequest>
    {
        public UploadFieldImagesRequestValidator()
        {
            RuleForEach(x => x.Images)
                .SetValidator(new FileSizeValidator());

            RuleForEach(x => x.Images)
                .SetValidator(new FileContentValidator());

            RuleFor(x => x.Images)
                .Must(images => images.Count <= 3)
                .WithMessage("Maximum 3 images are allowed")
                .When(x => x.Images is not null);

            RuleForEach(x => x.Images)
                .Must(image =>
                {
                    var extension = Path.GetExtension(image.FileName.ToLower());
                    return FileSettings.allowedImageExtensions.Contains(extension);
                })
                .WithMessage("File extension is not allowed")
                .When(x => x.Images is not null);
        }
    }
}
