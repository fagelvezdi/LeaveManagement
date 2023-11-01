using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "e1b4ca4f-5800-4ec9-85a2-d187f972abe0",
                    UserId = "6cb19953-5381-423b-95ce-d67f2ed469b6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "807f6e3b-82f1-4948-8a04-3009eb4411f8",
                    UserId = "66eefcbc-bca0-434d-98c8-0fb8151c2203"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "807f6e3b-82f1-4948-8a04-3009eb4411f8",
                    UserId = "4234d603-5c33-42fa-9ea3-864d3d11f817"
                }); 
        }
    }
}
