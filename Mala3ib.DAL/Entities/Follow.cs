namespace Mala3ib.DAL.Entities
{
    public class Follow
    {
        public bool IsDeleted { get; set; }
        public string FollowerId { get; set; } = string.Empty;
        public ApplicationUser Follower { get; set; } = default!;

        public string FollowingId { get; set; } = string.Empty;
        public ApplicationUser Following { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }
}
