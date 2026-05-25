
namespace Mala3ib.DAL.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<FieldReview> FieldReviews { get; set; } = new List<FieldReview>();

    }
}