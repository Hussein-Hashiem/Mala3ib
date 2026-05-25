
namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class FieldConfigurations : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Location)
                .HasMaxLength(100);

            builder.Property(x => x.PricePerHour)
                .HasPrecision(10, 2);

            builder.Property(x => x.Status)
                .HasConversion<string>();
        }
    }
}
