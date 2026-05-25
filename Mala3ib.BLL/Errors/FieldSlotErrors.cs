namespace Mala3ib.BLL.Errors
{
    public static class FieldSlotErrors
    {
        public static Error NotFound
            = new Error("FieldSlot.NotFound", "FieldSlot not found", ErrorType.NotFound);

        public static Error Unauthorized
            = new Error("FieldSlot.Unauthorized", "You dont have access to this field", ErrorType.Unauthorized);

        public static Error NotAvialable
            = new Error("FieldSlot.NotAvialable", "This slot not avialable", ErrorType.Conflict);

    }
}