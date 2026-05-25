namespace Mala3ib.DAL.Entities
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public Status Status { get; set; }  
        public bool IsDeleted { get; set; } = false;
        public int FieldOwnerId { get; set; }
        public FieldOwner FieldOwner { get; set; } = default!;
        public ICollection<FieldImage> Images { get; set; } =  new List<FieldImage>();
        public ICollection<FieldSlot> Slots { get; set; }  =  new List<FieldSlot>();
        public ICollection<FieldReview> Reviews { get; set; }   = new List<FieldReview>();
    }
}