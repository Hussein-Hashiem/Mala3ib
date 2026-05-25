
namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class EmailVerficationOtpConfiguration : IEntityTypeConfiguration<EmailVerficationOtp>
    {
        public void Configure(EntityTypeBuilder<EmailVerficationOtp> builder)
        {
            builder.Property(x => x.Type)
                .HasConversion<string>();
        }
    }
}
