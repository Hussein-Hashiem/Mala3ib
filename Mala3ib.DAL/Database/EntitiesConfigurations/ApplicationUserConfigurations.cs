using Mala3ib.DAL.Abstraction.Const;

namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.OwnsMany(u => u.RefreshTokens)
            .ToTable("RefreshTokens")   
            .WithOwner()
            .HasForeignKey("UserId");

            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.AdminId,
                FirstName = "Hussein",
                LastName = "Hashiem",
                Email = DefaultUsers.AdminEmail,
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEK9+ZCJewA99s+Qy4Ny4C6CJvIJobh2DHHbhM24m3sZSSN45ekjgzyfL3tPKZZJlgA=="
            });
        }
    }
}
