using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "6cb19953-5381-423b-95ce-d67f2ed469b6",
                    Email = "fagelvezdi@gmail.com",
                    NormalizedEmail = "FAGELVEZDI@GMAIL.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "SystemAdmin",
                    NormalizedUserName = "SYSTEMADMIN",
                    PasswordHash = hasher.HashPassword(null, "Password1"),
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    Id = "66eefcbc-bca0-434d-98c8-0fb8151c2203",
                    Email = "fagd2545@gmail.com",
                    NormalizedEmail = "FAGD2545@GMAIL.COM",
                    FirstName = "System",
                    LastName = "User",
                    UserName= "SystemUser",
                    NormalizedUserName = "SYSTEMUSER",
                    PasswordHash = hasher.HashPassword(null, "Password1"),
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    Id = "4234d603-5c33-42fa-9ea3-864d3d11f817",
                    Email = "fagd@localhost.com",
                    NormalizedEmail = "FAGD@LOCALHOST.COM",
                    FirstName = "Fredy",
                    LastName = "Gelvez",
                    UserName = "FredyGelvez",
                    NormalizedUserName = "FREDYGELVEZ",
                    PasswordHash = hasher.HashPassword(null, "Password1"),
                    EmailConfirmed = true,
                });
        }
    }
}
