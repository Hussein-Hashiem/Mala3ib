namespace Mala3ib.BLL.Errors
{
    public static class FieldOwnerErrors
    {
        public static Error NotFound
            = new Error("FieldOwner.NotFound", "Field owner not found", ErrorType.NotFound);
    }
}
