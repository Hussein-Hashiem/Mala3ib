namespace Mala3ib.BLL.Errors
{
    public static class BookingErrors
    {
        public static Error NotFound
            = new Error("Booking.NotFound", "Booking not found", ErrorType.NotFound);

        public static Error Unauthorized
            = new Error("Booking.Unauthorized", "User does not have access to this booking", ErrorType.Unauthorized);

        public static Error SlotUnavailable
            = new Error("Booking.SlotUnavailable", "Field slot is not available for booking", ErrorType.Conflict);

        public static Error AlreadyJoined
            = new Error("Booking.AlreadyJoined", "Player already joined this field slot", ErrorType.Conflict);
    }
}
