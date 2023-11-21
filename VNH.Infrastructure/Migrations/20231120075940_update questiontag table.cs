using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatequestiontagtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionTagId",
                table: "Question");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "QuestionTag",
                type: "uniqueidentifier",
                nullable: true);

             }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuestionTag");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionTagId",
                table: "Question",
                type: "uniqueidentifier",
                nullable: true);

           }
    }
}
