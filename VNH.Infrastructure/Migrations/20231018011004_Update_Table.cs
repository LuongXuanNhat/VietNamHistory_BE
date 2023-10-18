using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail");

            migrationBuilder.DropTable(
                name: "QuestionReport");

            migrationBuilder.RenameColumn(
                name: "QuestionReportId",
                table: "QuestionReportDetail",
                newName: "ReportId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionReportDetail_QuestionReportId",
                table: "QuestionReportDetail",
                newName: "IX_QuestionReportDetail_ReportId");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "PostTags",
                type: "nvarchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            
            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Post_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tag_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Post_PostId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tag_TagId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "QuestionReportDetail",
                newName: "QuestionReportId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionReportDetail_ReportId",
                table: "QuestionReportDetail",
                newName: "IX_QuestionReportDetail_QuestionReportId");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "PostTags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)");

            migrationBuilder.CreateTable(
                name: "QuestionReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReport", x => x.Id);
                });

            
            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail",
                column: "QuestionReportId",
                principalTable: "QuestionReport",
                principalColumn: "Id");
        }
    }
}
