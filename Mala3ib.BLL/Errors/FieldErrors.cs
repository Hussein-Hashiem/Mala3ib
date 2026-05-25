namespace Mala3ib.BLL.Errors
{
    public static class FieldErrors
    {
        public static Error NotFound
            = new Error("Field.NotFound", "Field not found", ErrorType.NotFound);
        public static Error Unauthorized
            = new Error("Field.Unauthorized", "You dont have access to this field", ErrorType.Unauthorized);
        
        public static Error FieldImagesLimit
            = new Error("Field.UploadImages", "Maximum 3 images are allowed per field", ErrorType.BadRequest);
    public static Error FieldImagesNotFount
            = new Error("Field.FieldImageNotFount", "Image not found", ErrorType.BadRequest);
    }
}