
namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(x => new { x.FollowerId, x.FollowingId });

            builder.HasIndex(x => new { x.FollowerId, x.FollowingId })
                   .IsUnique();
        }
    }
}
