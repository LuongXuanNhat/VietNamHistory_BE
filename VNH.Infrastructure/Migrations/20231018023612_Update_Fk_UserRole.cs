using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Fk_UserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
            name: "FK_AppUserRoles_AspNetUsers_UserId",
            table: "UserRoles",
            column: "UserId",
            principalTable: "User", 
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            }
    }
}
