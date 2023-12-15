using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.InsertData(
                table: "Topic",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000000"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Lịch sử Cổ đại", null },
                    { new Guid("0228bc1a-09d8-49c1-a893-9b58fff09442"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chính sách đổi mới", null },
                    { new Guid("0549df33-daaf-4b25-8c80-f1ebf3ef8233"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Phong trào Duy Tân", null },
                    { new Guid("1a9c5fa8-dd86-402c-bdf9-6e0a13519b43"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Giao lưu văn hóa quốc tế", null },
                    { new Guid("2fc72e05-a0eb-4487-b5a0-da609e1a5356"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Nguyễn", null },
                    { new Guid("3e6a861e-7683-4a6e-a990-6e3629c44b83"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null },
                    { new Guid("5228013c-d05d-41bc-8706-f547b783e3d9"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ đổi mới", null },
                    { new Guid("5c047dbf-a6ef-48c7-a582-dd867f5e6e7a"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ thuộc địa Pháp", null },
                    { new Guid("7b673511-8385-4a5a-9788-7cdb1b1d0057"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Những thăng trầm của triều đại Lê", null },
                    { new Guid("7c061076-9a98-4eef-829b-47eb43bb58ba"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Lịch sử thời kỳ thuộc địa", null },
                    { new Guid("86855d15-2e3b-475a-89c5-726a06f24fc0"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Hậu Lê", null },
                    { new Guid("8703f2b4-2952-4936-b0ef-f95ee5144562"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Anh hùng", null },
                    { new Guid("8c2dc0b5-376c-4719-8ee9-a2f5cf247f68"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nghệ thuật và văn hóa đương đại", null },
                    { new Guid("8c5979fe-5d04-4495-a9ef-675f06a462b9"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ Trung đại", null },
                    { new Guid("bf8ef934-06b2-45ec-aff1-a1d20c7d50a2"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null },
                    { new Guid("c2e754b9-e397-432d-8032-af79f93532b1"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Vương quốc Âu Lạc.", null },
                    { new Guid("c80c4467-c398-406d-a91d-500b987219a3"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa và Nghệ thuật", null },
                    { new Guid("d5ccd739-1898-4cc6-bbb5-4f4df93c43e1"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Đời sống trong xã cổ Đông Sơn.", null },
                    { new Guid("ecbc1c53-18af-4c2b-b2ce-195f6e6f0006"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa dân gian", null },
                    { new Guid("f0db090d-2a6f-4c21-af27-ed8960249919"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Pháp-Đông Dương", null },
                    { new Guid("f133d506-4d93-4b04-bea5-f5d6fdebcfdd"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thách thức hiện đại hóa", null },
                    { new Guid("fa1a97b2-3819-40e1-bb94-1981dc75e702"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Triều đại Lý.", null }
                });

             }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
