using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Presenters.Migrations
{
    public partial class Add_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"), "d12c03db-90a8-4400-8efa-40ca011a87c3", "student", "student" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), "6da30627-e97b-4de1-9573-5615ecd29697", "admin", "admin" },
                    { new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"), "8700a897-d7aa-4013-8688-5f8a83b461a5", "teacher", "teacher" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Fullname", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), 0, "985e159a-97b0-4efc-932c-ec1cbe1eb46d", new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Local), "admin@gmail.com", true, "Lương Xuân Nhất", 0, false, null, "onionwebdev@gmail.com", "admin", "AQAAAAEAACcQAAAAEFEsCTKz2Ne33x4x/IjWtPdicJ27lUm2gzrUkAaiVonckLc08tCz772f1XRZ3n33nw==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d1f771da-b318-42f8-a003-5a15614216f5"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5") });

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });
        }
    }
}
