namespace Mala3ib.DAL.Entities
{
    public class FieldSlotPlayer
    {
        public int Id { get; set; }

        public int FieldSlotId { get; set; }
        public FieldSlot FieldSlot { get; set; } = default!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;

        public bool IsCaptain { get; set; } = false;
    }
}