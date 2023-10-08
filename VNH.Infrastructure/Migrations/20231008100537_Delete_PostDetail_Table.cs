using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Delete_PostDetail_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostDetail");

           }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentNumber = table.Column<int>(type: "int", nullable: true),
                    LikeNumber = table.Column<int>(type: "int", nullable: true),
                    SaveNumber = table.Column<int>(type: "int", nullable: true),
                    ViewNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostDetai__PostI__0D7A0286",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostDetai__UserI__0E6E26BF",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateIndex(
                name: "IX_PostDetail_PostId",
                table: "PostDetail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostDetail_UserId",
                table: "PostDetail",
                column: "UserId");
        }
    }
}
