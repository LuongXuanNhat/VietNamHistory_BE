using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatequestiontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PubDate",
                table: "Question",
                newName: "UpdateAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Question",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

             }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Question",
                newName: "PubDate");

            }
    }
}
