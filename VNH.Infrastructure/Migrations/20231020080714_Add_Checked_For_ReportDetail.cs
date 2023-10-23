using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Checked_For_ReportDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "QuestionReportDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "PostReportDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Report",
                columns: new[] { "Id", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8251), "Sử dụng khi bạn thấy nội dung bài đăng chứa lời lẽ xúc phạm, kỳ thị hoặc có tính chất đe doạ đến người khác.", "Nội dung xấu, xúc phạm, hay kỳ thị" },
                    { new Guid("3043c693-b3c9-453e-9876-31c943222576"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8262), "Dùng khi bạn muốn báo cáo vì nó quá nhiều thông báo hoặc quảng cáo không mong muốn.", "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn" },
                    { new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8255), "Sử dụng khi bạn thấy nội dung chứa hình ảnh tự tử hoặc khuyến khích hành vi tự gây thương tổn.", "Chứa nội dung tự tử hoặc tự gây thương tổn" },
                    { new Guid("4a780087-9058-41c9-b84b-944d1a502010"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8259), "Sử dụng khi bạn thấy rằng nội dung chứa thông tin sai lệch, giả mạo hoặc vi phạm quy tắc về sự thật và trung thực.", "Bài đăng chứa thông tin sai lệch hoặc giả mạo" },
                    { new Guid("bab1da58-6921-44b9-837f-c58d3998497b"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8253), "Dùng khi bạn thấy nội dung chứa hình ảnh hoặc video bạo lực hoặc đội nhóm xấu, hoặc khuyến khích hành vi bạo lực.", "Chứa nội dung bạo lực hoặc đội nhóm xấu" },
                    { new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8258), "Sử dụng khi bạn cho rằng Nội dung vi phạm quyền sở hữu trí tuệ hoặc bản quyền, chẳng hạn như sử dụng hình ảnh hoặc video mà bạn sở hữu mà không có sự cho phép.", "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ" },
                    { new Guid("d30e1353-0163-43c1-b757-7957981b0eda"), new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8233), " Báo cáo này được sử dụng khi người dùng chia sẻ nội dung cá nhân của bạn mà bạn cho rằng vi phạm quyền riêng tư của bạn.", "Nội dung vi phạm quy định về quyền riêng tư" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("3043c693-b3c9-453e-9876-31c943222576"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("4a780087-9058-41c9-b84b-944d1a502010"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("bab1da58-6921-44b9-837f-c58d3998497b"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"));

            migrationBuilder.DeleteData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("d30e1353-0163-43c1-b757-7957981b0eda"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d1f771da-b318-42f8-a003-5a15614216f5"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5") });

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "QuestionReportDetail");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "PostReportDetail");
        }
    }
}
