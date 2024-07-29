using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities.ConfigurationModels;

namespace Events.Infrastructure.DbContext.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "03eb8fb1-81f2-4e61-85f8-2ab132bb8597",
                    Name = UserRoles.Admin,
                    NormalizedName = UserRoles.AdminNormalized
                },
                new IdentityRole
                {
                    Id = "6a7cc4af-8709-45c1-ba61-7413c0f07a1b",
                    Name = UserRoles.User,
                    NormalizedName = UserRoles.UserNormalized
                }
                );
        }
    }
}
