namespace Mala3ib.DAL.Entities
{
    public class FieldOwner
    {
        public int Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Status Status { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<Field> Fields { get; set; } = new List<Field>();
    }
}