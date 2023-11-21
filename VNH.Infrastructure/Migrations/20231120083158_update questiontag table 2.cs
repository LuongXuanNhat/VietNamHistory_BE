using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatequestiontagtable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__QuestionTag__Id__2739D489",
                table: "QuestionTag");

         
            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_QuestionId",
                table: "QuestionTag",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionTag__Id__2739D489",
                table: "QuestionTag",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__QuestionTag__Id__2739D489",
                table: "QuestionTag");

            migrationBuilder.DropIndex(
                name: "IX_QuestionTag_QuestionId",
                table: "QuestionTag");

          
            migrationBuilder.AddForeignKey(
                name: "FK__QuestionTag__Id__2739D489",
                table: "QuestionTag",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id");
        }
    }
}
