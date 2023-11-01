using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "e1b4ca4f-5800-4ec9-85a2-d187f972abe0",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
            },

            new IdentityRole
            {
                Id = "807f6e3b-82f1-4948-8a04-3009eb4411f8",
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
            });
        }
    }
}
