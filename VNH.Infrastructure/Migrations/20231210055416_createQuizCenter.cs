using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createQuizCenter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Quiz__Id__29221CFB",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Answer1",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Answer2",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Answer3",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Answer4",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "RightAnswer",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Quiz",
                newName: "Content");

            migrationBuilder.AddColumn<Guid>(
                name: "MultipleChoiceId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuizAnswerId",
                table: "Quiz",
                type: "uniqueidentifier",
                maxLength: 500,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MultipleChoise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    WorkTime = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoise", x => x.Id);
                    table.ForeignKey(
                        name: "FK__MultipleChoice__UserId__06CD04F7",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCorrect = table.Column<bool>(type: "bit", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuizAnswers__QuizId__1AD3FVA4",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MultipleChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scores = table.Column<int>(type: "int", nullable: false),
                    CompletionTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    StarDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamHistory_MultipleChoise_MultipleChoiceId",
                        column: x => x.MultipleChoiceId,
                        principalTable: "MultipleChoise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Quiz_MultipleChoiceId",
                table: "Quiz",
                column: "MultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory",
                column: "MultipleChoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoise_UserId",
                table: "MultipleChoise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswer_QuizId",
                table: "QuizAnswer",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK__MultipleChoice__Id__07C12930",
                table: "Quiz",
                column: "MultipleChoiceId",
                principalTable: "MultipleChoise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__MultipleChoice__Id__07C12930",
                table: "Quiz");

            migrationBuilder.DropTable(
                name: "ExamHistory");

            migrationBuilder.DropTable(
                name: "QuizAnswer");

            migrationBuilder.DropTable(
                name: "MultipleChoise");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_MultipleChoiceId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "MultipleChoiceId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "QuizAnswerId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Quiz",
                newName: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Answer1",
                table: "Quiz",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Answer2",
                table: "Quiz",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Answer3",
                table: "Quiz",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Answer4",
                table: "Quiz",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightAnswer",
                table: "Quiz",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

         
            migrationBuilder.AddForeignKey(
                name: "FK__Quiz__Id__29221CFB",
                table: "Quiz",
                column: "Id",
                principalTable: "Exercise",
                principalColumn: "Id");
        }
    }
}
