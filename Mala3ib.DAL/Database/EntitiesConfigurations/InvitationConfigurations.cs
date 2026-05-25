namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class InvitationConfigurations : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<string>();

            builder.Property(i => i.Type)
            .IsRequired()
            .HasConversion<string>();
        }
    }
}