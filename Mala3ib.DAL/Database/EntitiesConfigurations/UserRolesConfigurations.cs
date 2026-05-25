using Mala3ib.DAL.Abstraction.Const;
using Microsoft.AspNetCore.Identity;

namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class UserRolesConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                UserId = DefaultUsers.AdminId,
                RoleId = DefaultRoles.AdminRoleId,
            });
        }
    }
}
