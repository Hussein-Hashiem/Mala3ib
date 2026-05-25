namespace Mala3ib.DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public BookingStatus Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int FieldSlotId { get; set; }
        public FieldSlot FieldSlot { get; set; } = default!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;
    }
}