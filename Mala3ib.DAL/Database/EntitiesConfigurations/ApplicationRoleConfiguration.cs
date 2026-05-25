
using Mala3ib.DAL.Abstraction.Const;

namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                new ApplicationRole()
                {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
                },
                new ApplicationRole()
                {
                    Id = DefaultRoles.FieldOwnerRoleId,
                    Name = DefaultRoles.FieldOwner,
                    NormalizedName = DefaultRoles.FieldOwner.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.FieldOwnerRoleConcurrencyStamp
                },
                new ApplicationRole()
                {
                    Id = DefaultRoles.PlayerRoleId,
                    Name = DefaultRoles.Player,
                    NormalizedName = DefaultRoles.Player.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.PlayerRoleConcurrencyStamp
                }
            ]);
        }
    }
}
