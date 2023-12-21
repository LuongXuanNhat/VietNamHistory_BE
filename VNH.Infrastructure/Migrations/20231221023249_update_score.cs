using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_score : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<float>(
                name: "Scores",
                table: "ExamHistory",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

          }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AlterColumn<int>(
                name: "Scores",
                table: "ExamHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            }
    }
}
