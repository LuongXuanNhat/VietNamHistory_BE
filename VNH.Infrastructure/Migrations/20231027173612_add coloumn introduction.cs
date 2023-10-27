using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcoloumnintroduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("3043c693-b3c9-453e-9876-31c943222576"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8991));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("4a780087-9058-41c9-b84b-944d1a502010"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8995));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("bab1da58-6921-44b9-837f-c58d3998497b"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8992));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("d30e1353-0163-43c1-b757-7957981b0eda"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 28, 0, 36, 12, 172, DateTimeKind.Local).AddTicks(8976));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"),
                column: "ConcurrencyStamp",
                value: "21c472c4-52d8-4820-80fb-e07c02a62c15");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                column: "ConcurrencyStamp",
                value: "ff4be4a5-bc81-466a-9a52-1d08970b2f8e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"),
                column: "ConcurrencyStamp",
                value: "95ed3587-03d0-44ed-bcc6-3695e132d751");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d1f771da-b318-42f8-a003-5a15614216f5"),
                columns: new[] { "ConcurrencyStamp", "Introduction", "PasswordHash" },
                values: new object[] { "9c07b050-dd13-40c4-9433-5facf1068a4f", null, "AQAAAAEAACcQAAAAEKz2B1KxaaWRrgN1Hd3PTk0yb+SZB79xTOlQKS1TgAI6wMNly4XoAVeOlFqnvsU8Iw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "User");

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8251));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("3043c693-b3c9-453e-9876-31c943222576"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8262));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8255));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("4a780087-9058-41c9-b84b-944d1a502010"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8259));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("bab1da58-6921-44b9-837f-c58d3998497b"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8253));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8258));

            migrationBuilder.UpdateData(
                table: "Report",
                keyColumn: "Id",
                keyValue: new Guid("d30e1353-0163-43c1-b757-7957981b0eda"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 20, 15, 7, 14, 489, DateTimeKind.Local).AddTicks(8233));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"),
                column: "ConcurrencyStamp",
                value: "cdd07513-a753-4446-bb2e-fdf342432274");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                column: "ConcurrencyStamp",
                value: "8f306177-bd4a-4f3d-812f-774927febc0a");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"),
                column: "ConcurrencyStamp",
                value: "ef4bd6d1-c7b9-4013-93ec-2614e3d532a4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d1f771da-b318-42f8-a003-5a15614216f5"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "311ace0d-cd72-4127-9f17-ae26c351e8c3", "AQAAAAEAACcQAAAAEDUOWq3UqyNeXmAlz5MOnvKn0UfK+m9H7SYBn0R02ctctjwJsE941TRCPRhvLl8i1A==" });
        }
    }
}
