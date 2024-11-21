using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "d1e5d9c2-7bcd-4ae8-b2f5-94ecaff72384", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "cd028594-76df-41bb-b8ad-9d3cdecad743", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }
    }
}
