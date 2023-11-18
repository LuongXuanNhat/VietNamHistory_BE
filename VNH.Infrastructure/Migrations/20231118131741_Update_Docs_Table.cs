using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Docs_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Document");

            migrationBuilder.AddColumn<string>(
                name: "SubId",
                table: "Document",
                type: "varchar(500)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubId",
                table: "Document");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Document",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Document",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

             }
    }
}
