namespace Mala3ib.DAL.Entities
{
    public class Invitation
    {
        public int Id { get; set; }
        public InvitationStatus Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public int SenderId { get; set; }
        public Player Sender { get; set; } = default!;
        public int RecieverId { get; set; }
        public Player Reciever { get; set; } = default!;
        public int FieldSlotId { get; set; }
        public FieldSlot FieldSlot { get; set; } = default!;
        public InvitationType Type { get; set; }
    }
}