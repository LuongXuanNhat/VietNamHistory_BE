using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class u_multi_exam_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamHistory_MultipleChoise_MultipleChoiceId",
                table: "ExamHistory");

            migrationBuilder.DropIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory");

           
            migrationBuilder.CreateIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory",
                column: "MultipleChoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK__MultipleChoice__ExamHistoryId__06CD04F7",
                table: "ExamHistory",
                column: "MultipleChoiceId",
                principalTable: "MultipleChoise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__MultipleChoice__ExamHistoryId__06CD04F7",
                table: "ExamHistory");

            migrationBuilder.DropIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory");

            migrationBuilder.InsertData(
                table: "Report",
                columns: new[] { "Id", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(393), "Sử dụng khi bạn thấy nội dung bài đăng chứa lời lẽ xúc phạm, kỳ thị hoặc có tính chất đe doạ đến người khác.", "Nội dung xấu, xúc phạm, hay kỳ thị" },
                    { new Guid("3043c693-b3c9-453e-9876-31c943222576"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(411), "Dùng khi bạn muốn báo cáo vì nó quá nhiều thông báo hoặc quảng cáo không mong muốn.", "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn" },
                    { new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(403), "Sử dụng khi bạn thấy nội dung chứa hình ảnh tự tử hoặc khuyến khích hành vi tự gây thương tổn.", "Chứa nội dung tự tử hoặc tự gây thương tổn" },
                    { new Guid("4a780087-9058-41c9-b84b-944d1a502010"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(408), "Sử dụng khi bạn thấy rằng nội dung chứa thông tin sai lệch, giả mạo hoặc vi phạm quy tắc về sự thật và trung thực.", "Bài đăng chứa thông tin sai lệch hoặc giả mạo" },
                    { new Guid("bab1da58-6921-44b9-837f-c58d3998497b"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(400), "Dùng khi bạn thấy nội dung chứa hình ảnh hoặc video bạo lực hoặc đội nhóm xấu, hoặc khuyến khích hành vi bạo lực.", "Chứa nội dung bạo lực hoặc đội nhóm xấu" },
                    { new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(406), "Sử dụng khi bạn cho rằng Nội dung vi phạm quyền sở hữu trí tuệ hoặc bản quyền, chẳng hạn như sử dụng hình ảnh hoặc video mà bạn sở hữu mà không có sự cho phép.", "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ" },
                    { new Guid("d30e1353-0163-43c1-b757-7957981b0eda"), new DateTime(2023, 12, 22, 13, 54, 20, 961, DateTimeKind.Local).AddTicks(380), " Báo cáo này được sử dụng khi người dùng chia sẻ nội dung cá nhân của bạn mà bạn cho rằng vi phạm quyền riêng tư của bạn.", "Nội dung vi phạm quy định về quyền riêng tư" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"), "fd6f7cf8-8977-4b47-85a9-22d25d605913", "student", "student" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), "7384f037-6d8d-48a6-a154-195d3a5db7ab", "admin", "admin" },
                    { new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"), "a6163b8e-97ec-498b-8fd9-e465ff3543e1", "teacher", "teacher" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Fullname", "Gender", "Image", "Introduction", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NumberConfirm", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), 0, "655f7435-dbfe-418e-a873-2a98f7518f62", new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Local), "admin@gmail.com", true, "Lương Xuân Nhất", 0, "", null, false, false, null, "onionwebdev@gmail.com", "admin", null, "AQAAAAEAACcQAAAAEOtQFl2yzdrxjnrsTdsQC9KSysBJv5jjIYf4CZbwHIO7kgkPifjNSf1I4MigYSkbRw==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5") });

            migrationBuilder.InsertData(
                table: "Topic",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000000"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Lịch sử Cổ đại", null },
                    { new Guid("208c6340-0a32-492f-afcb-e522ee62a379"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ đổi mới", null },
                    { new Guid("30006f7c-d22b-4f57-a4a0-2004bbd7cc5d"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Đời sống trong xã cổ Đông Sơn.", null },
                    { new Guid("301eba8b-5332-4501-ae39-4a0475784012"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Anh hùng", null },
                    { new Guid("3167df9b-f1d8-4da8-a719-2d9c72618788"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Phong trào Duy Tân", null },
                    { new Guid("3a5050ee-fc0c-4063-b81e-4dbd58bd1bf2"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ thuộc địa Pháp", null },
                    { new Guid("45b5a102-1a0f-4a01-b3ad-87033c628a74"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa và Nghệ thuật", null },
                    { new Guid("493af5b6-a3f9-45b4-a2e4-25feef7ad6d8"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa dân gian", null },
                    { new Guid("52463519-05b1-4ad8-a501-ce6073592f7b"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Những thăng trầm của triều đại Lê", null },
                    { new Guid("570b164e-e22e-464c-b224-d2a0309e3065"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nghệ thuật và văn hóa đương đại", null },
                    { new Guid("5c414d41-5620-48a4-88c7-61c9a0174243"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Lịch sử thời kỳ thuộc địa", null },
                    { new Guid("68b43a88-5ef2-494a-b0fe-5bb1705e0736"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Hậu Lê", null },
                    { new Guid("80bc02ad-9401-4280-a76d-9fa5f7813dd1"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ Trung đại", null },
                    { new Guid("8b2d71a5-ccfd-45ae-9bf3-565b56b73931"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Vương quốc Âu Lạc.", null },
                    { new Guid("92d0fb18-3849-46a7-b6b0-fe76898549a1"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chính sách đổi mới", null },
                    { new Guid("987d7996-c324-4585-b86a-cb2a13e2171d"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Nguyễn", null },
                    { new Guid("cc7d45d9-1ffb-460e-9966-8a81e8803896"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null },
                    { new Guid("d4a56cbc-e450-4323-be8f-a0606ed5dfe5"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thách thức hiện đại hóa", null },
                    { new Guid("dd41bdc9-d9d0-4e4f-a8f7-7066fbda0cdc"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Giao lưu văn hóa quốc tế", null },
                    { new Guid("e932683e-fa11-4adb-8409-93fb40e23315"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Pháp-Đông Dương", null },
                    { new Guid("faa13d37-2f3a-447b-92ab-0a26126273e8"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Triều đại Lý.", null },
                    { new Guid("fd826d64-1f87-436d-82c9-7009d2571199"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory",
                column: "MultipleChoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamHistory_MultipleChoise_MultipleChoiceId",
                table: "ExamHistory",
                column: "MultipleChoiceId",
                principalTable: "MultipleChoise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
