namespace Mala3ib.DAL.Entities
{
    public class FieldImage
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public int FieldId { get; set; }
        public Field Field { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
    }
}