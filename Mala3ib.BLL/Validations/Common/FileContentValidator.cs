namespace Mala3ib.BLL.Validations.Common
{
    public class FileContentValidator : AbstractValidator<IFormFile>
    {
        public FileContentValidator()
        {
            RuleFor(x => x)
               .Must((request, context) =>
               {
                   BinaryReader binary = new(request.OpenReadStream());
                   var bytes = binary.ReadBytes(2);

                   var fileSequenceHex = BitConverter.ToString(bytes);

                   foreach (var signature in FileSettings.BlockedSignatures)
                   {
                       if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                           return false;
                   }
                   return true;
               })
               .WithMessage("Not allowed file content")
               .When(x => x is not null);
        }
    }
}
