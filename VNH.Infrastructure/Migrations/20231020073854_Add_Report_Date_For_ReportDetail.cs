using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Report_Date_For_ReportDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__Repor__151B244E",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__2180FB33",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail");

            
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "QuestionReportDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PostReportDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "PostReportDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "PostReportDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__Repor__151B244E",
                table: "PostReportDetail",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__2180FB33",
                table: "QuestionReportDetail",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__Repor__151B244E",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__2180FB33",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "QuestionReportDetail");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "PostReportDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionReportDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PostReportDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "PostReportDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

           
            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__Repor__151B244E",
                table: "PostReportDetail",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__2180FB33",
                table: "QuestionReportDetail",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__Quest__22751F6C",
                table: "QuestionReportDetail",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");
        }
    }
}
