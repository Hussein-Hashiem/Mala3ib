
namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class FieldSlotConfigurations : IEntityTypeConfiguration<FieldSlot>
    {
        public void Configure(EntityTypeBuilder<FieldSlot> builder)
        {
            builder.Property(x => x.Price)
                .HasPrecision(10, 2);
        }
    }
}
