using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_content_report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Report",
                columns: new[] { "Id", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("1238884a-e607-4404-b75e-857b505e743e"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4384), "Dùng khi bạn thấy nội dung chứa hình ảnh hoặc video bạo lực hoặc đội nhóm xấu, hoặc khuyến khích hành vi bạo lực.", "Chứa nội dung bạo lực hoặc đội nhóm xấu" },
                    { new Guid("77051145-b23f-4dff-a085-558cef7fb308"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4395), "Sử dụng khi bạn thấy nội dung chứa hình ảnh tự tử hoặc khuyến khích hành vi tự gây thương tổn.", "Chứa nội dung tự tử hoặc tự gây thương tổn" },
                    { new Guid("7a8ea6de-a8ad-46e7-87a8-320ef172c92a"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4398), "Sử dụng khi bạn cho rằng Nội dung vi phạm quyền sở hữu trí tuệ hoặc bản quyền, chẳng hạn như sử dụng hình ảnh hoặc video mà bạn sở hữu mà không có sự cho phép.", "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ" },
                    { new Guid("91c52180-68bf-40cc-b9d4-451e9d7cafd8"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4380), "Sử dụng khi bạn thấy nội dung bài đăng chứa lời lẽ xúc phạm, kỳ thị hoặc có tính chất đe doạ đến người khác.", "Nội dung xấu, xúc phạm, hay kỳ thị" },
                    { new Guid("9cc193cd-87ae-4461-a708-5812ecdc419a"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4363), " Báo cáo này được sử dụng khi người dùng chia sẻ nội dung cá nhân của bạn mà bạn cho rằng vi phạm quyền riêng tư của bạn.", "Nội dung vi phạm quy định về quyền riêng tư" },
                    { new Guid("af65cb27-c071-4a40-9eb3-229f3c6a31c0"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4403), "Dùng khi bạn muốn báo cáo vì nó quá nhiều thông báo hoặc quảng cáo không mong muốn.", "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn" },
                    { new Guid("f34f82ed-9f42-46cd-b8af-556d355ead56"), new DateTime(2023, 10, 20, 10, 20, 0, 417, DateTimeKind.Local).AddTicks(4400), "Sử dụng khi bạn thấy rằng nội dung chứa thông tin sai lệch, giả mạo hoặc vi phạm quy tắc về sự thật và trung thực.", "Bài đăng chứa thông tin sai lệch hoặc giả mạo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
