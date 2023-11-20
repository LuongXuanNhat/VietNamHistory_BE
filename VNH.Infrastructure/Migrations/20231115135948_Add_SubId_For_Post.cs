using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_SubId_For_Post : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "SubId",
                table: "Post",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true);

            
            migrationBuilder.AddForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "SubId",
                table: "Post");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            
            migrationBuilder.AddForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");
        }
    }
}
