namespace Mala3ib.DAL.Entities
{
    public class FieldReview
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime DateTime { get; set; } 
        public int FieldId { get; set; }
        public Field Field { get; set; } = default!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;
    }
}