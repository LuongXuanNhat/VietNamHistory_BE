using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Image_Attribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlImage",
                table: "Document",
                newName: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "User",
                type: "varchar(max)",
                maxLength: 3145728,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldMaxLength: 3145728,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Post",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Document",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Course",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5947));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("3043c693-b3c9-453e-9876-31c943222576"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5961));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5953));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("4a780087-9058-41c9-b84b-944d1a502010"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5959));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("bab1da58-6921-44b9-837f-c58d3998497b"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5949));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5956));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("d30e1353-0163-43c1-b757-7957981b0eda"),
                column: "CreatedAt",
                value: new DateTime(2023, 11, 9, 22, 49, 50, 982, DateTimeKind.Local).AddTicks(5927));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Document",
                newName: "UrlImage");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "User",
                type: "varbinary(max)",
                maxLength: 3145728,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 3145728,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Post",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Document",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6217));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("3043c693-b3c9-453e-9876-31c943222576"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6223));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("4a780087-9058-41c9-b84b-944d1a502010"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6227));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("bab1da58-6921-44b9-837f-c58d3998497b"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6226));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("d30e1353-0163-43c1-b757-7957981b0eda"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 30, 22, 57, 18, 543, DateTimeKind.Local).AddTicks(6204));

        }
    }
}
