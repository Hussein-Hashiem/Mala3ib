using Mala3ib.DAL.Abstraction.Const;

namespace Mala3ib.DAL.Database.EntitiesConfigurations
{
    public class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasData(new Admin
            {
                Id = 1,
                UserId = DefaultUsers.AdminId,
            });
        }
    }
}
