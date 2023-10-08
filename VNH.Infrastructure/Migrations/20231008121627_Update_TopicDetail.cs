using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_TopicDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__TopicDeta__TagId__07C12930",
                table: "TopicDetail");

            migrationBuilder.DropIndex(
                name: "IX_TopicDetail_TagId",
                table: "TopicDetail");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "TopicDetail");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "TopicDetail",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_TopicDetail_PostId",
                table: "TopicDetail",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK__TopicDeta__TagId__07C12930",
                table: "TopicDetail",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__TopicDeta__TagId__07C12930",
                table: "TopicDetail");

            migrationBuilder.DropIndex(
                name: "IX_TopicDetail_PostId",
                table: "TopicDetail");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "TopicDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "TopicDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

          
            migrationBuilder.CreateIndex(
                name: "IX_TopicDetail_TagId",
                table: "TopicDetail",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK__TopicDeta__TagId__07C12930",
                table: "TopicDetail",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id");
        }
    }
}
