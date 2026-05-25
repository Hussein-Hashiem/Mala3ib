
namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class FieldReviewConfigurations : IEntityTypeConfiguration<FieldReview>
    {
        public void Configure(EntityTypeBuilder<FieldReview> builder)
        {
            builder.Property(x => x.Comment)
                .HasMaxLength(500);
        }
    }
}
