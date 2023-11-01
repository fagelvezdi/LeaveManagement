using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Identity.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66eefcbc-bca0-434d-98c8-0fb8151c2203",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "507b0155-5aac-4d08-b347-00ba6eff71f7", "AQAAAAIAAYagAAAAEHP3ud6zAEuu075JDe/iupYrMnWhsGdxSEiiRjkHF8sX6THFIvR7qGj8QUV6CNt/+w==", "e5bf6aca-7e0c-4b0f-bdbc-3d8345f5b761", "SystemUser" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6cb19953-5381-423b-95ce-d67f2ed469b6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "54d4af8e-4c33-462a-9615-b37f6c80ef46", "AQAAAAIAAYagAAAAEIrctZjI93OobIw2Ka+bkkGYceotxsb9CBU5ZspXwSj+wi4rpob8Blu25HfHIelkQw==", "dce618c2-ab95-45d7-952e-b6f00a379322", "SystemAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4234d603-5c33-42fa-9ea3-864d3d11f817", 0, "2f4faeb9-bb2e-4ed1-b03f-02d6fa0e2120", "fagd@localhost.com", true, "Fredy", "Gelvez", false, null, "FAGD@LOCALHOST.COM", "FREDYGELVEZ", "AQAAAAIAAYagAAAAEAK3R+mFDD+vyH9cquDzTD46b55UYOSWkZBMqUrTs6bvvXPdiNeVvWDFj63j6k7Kyw==", null, false, "1a755d8f-cfb5-44fc-9bb2-5bc948df6d67", false, "FredyGelvez" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "807f6e3b-82f1-4948-8a04-3009eb4411f8", "4234d603-5c33-42fa-9ea3-864d3d11f817" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "807f6e3b-82f1-4948-8a04-3009eb4411f8", "4234d603-5c33-42fa-9ea3-864d3d11f817" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4234d603-5c33-42fa-9ea3-864d3d11f817");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66eefcbc-bca0-434d-98c8-0fb8151c2203",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "99d651ef-49ee-4990-8b58-fffa8a2ed4e2", "AQAAAAIAAYagAAAAEGOUm5QB1e+bTAr003cN1wxdcO1BC0NNse6rQfi7uj7SMPEGuG0WVyijHg5hnKq8Og==", "b4d6fcaf-d36a-4fc9-bd6b-0ea150a49329", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6cb19953-5381-423b-95ce-d67f2ed469b6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "8c8a61dc-09b0-4878-a910-bf7a181070d1", "AQAAAAIAAYagAAAAEO7WiS/3hdzkKk5OoxU9vfJNgBIni4kXlMR0ra09YGb//DAFBKQtGBBG4InZUwkt+Q==", "9e643b18-69e7-4407-a6e4-71d76e1591db", null });
        }
    }
}
