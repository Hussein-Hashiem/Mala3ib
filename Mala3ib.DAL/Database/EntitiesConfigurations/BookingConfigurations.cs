namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<string>();
        }
    }
}