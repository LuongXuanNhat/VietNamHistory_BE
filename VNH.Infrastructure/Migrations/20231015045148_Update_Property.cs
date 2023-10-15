using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.DropForeignKey(
                name: "FK__Answer__AuthorId__1AD3FDA4",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK__AnswerVot__UserI__1EA48E88",
                table: "AnswerVote");

            migrationBuilder.DropForeignKey(
                name: "FK__Course__UserId__787EE5A0",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseCom__UserI__797309D9",
                table: "CourseComment");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseRat__UserI__7E37BEF6",
                table: "CourseRating");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseSav__UserI__03F0984C",
                table: "CourseSave");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseSub__UserI__7B5B524B",
                table: "CourseSubComment");

            migrationBuilder.DropForeignKey(
                name: "FK__Document__UserId__0A9D95DB",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK__DocumentS__UserI__0B91BA14",
                table: "DocumentSave");

            migrationBuilder.DropForeignKey(
                name: "FK__ExerciseD__UserI__01142BA1",
                table: "ExerciseDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__NotificationDetail__UserId__1EA48E88",
                table: "NotificationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__Post__TopicId__76969D2E",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK__PostComme__UserI__10566F31",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK__PostLike__UserId__14270015",
                table: "PostLike");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PostSave__UserId__09A971A2",
                table: "PostSave");

            migrationBuilder.DropForeignKey(
                name: "FK__PostSubCo__UserI__123EB7A3",
                table: "PostSubComment");

            migrationBuilder.DropForeignKey(
                name: "FK__Question__Author__18EBB532",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionL__UserI__208CD6FA",
                table: "QuestionLike");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionS__UserI__245D67DE",
                table: "QuestionSave");

            migrationBuilder.DropForeignKey(
                name: "FK__Search__UserId__17F790F9",
                table: "Search");

            migrationBuilder.DropForeignKey(
                name: "FK__SubAnswer__Autho__1CBC4616",
                table: "SubAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK__TopicDeta__Topic__06CD04F7",
                table: "TopicDetail");




        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Answer__AuthorId__1AD3FDA4",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK__AnswerVot__UserI__1EA48E88",
                table: "AnswerVote");

            migrationBuilder.DropForeignKey(
                name: "FK__Course__UserId__787EE5A0",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseCom__UserI__797309D9",
                table: "CourseComment");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseRat__UserI__7E37BEF6",
                table: "CourseRating");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseSav__UserI__03F0984C",
                table: "CourseSave");

            migrationBuilder.DropForeignKey(
                name: "FK__CourseSub__UserI__7B5B524B",
                table: "CourseSubComment");

            migrationBuilder.DropForeignKey(
                name: "FK__Document__UserId__0A9D95DB",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK__DocumentS__UserI__0B91BA14",
                table: "DocumentSave");

            migrationBuilder.DropForeignKey(
                name: "FK__ExerciseD__UserI__01142BA1",
                table: "ExerciseDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__NotificationDetail__UserId__1EA48E88",
                table: "NotificationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__Post__TopicId__76969D2E",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK__PostComme__UserI__10566F31",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK__PostLike__UserId__14270015",
                table: "PostLike");

            migrationBuilder.DropForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PostSave__UserId__09A971A2",
                table: "PostSave");

            migrationBuilder.DropForeignKey(
                name: "FK__PostSubCo__UserI__123EB7A3",
                table: "PostSubComment");

            migrationBuilder.DropForeignKey(
                name: "FK__Question__Author__18EBB532",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionL__UserI__208CD6FA",
                table: "QuestionLike");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__QuestionS__UserI__245D67DE",
                table: "QuestionSave");

            migrationBuilder.DropForeignKey(
                name: "FK__Search__UserId__17F790F9",
                table: "Search");

            migrationBuilder.DropForeignKey(
                name: "FK__SubAnswer__Autho__1CBC4616",
                table: "SubAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK__TopicDeta__Topic__06CD04F7",
                table: "TopicDetail");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "TopicId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "UserShort",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", maxLength: 3145728, nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberConfirm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopicName",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicName", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Topic__AuthorId__05D8E0BE",
                        column: x => x.AuthorId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

           

            migrationBuilder.InsertData(
                table: "UserShort",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Fullname", "Gender", "Image", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NumberConfirm", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), 0, "12148b6f-9ac9-4994-ad50-54e01ec9ebaf", new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Local), "admin@gmail.com", true, "Lương Xuân Nhất", 0, null, false, null, "onionwebdev@gmail.com", "admin", null, "AQAAAAEAACcQAAAAEAUppGPkCUVQwexu08dnuALfftIE+P1ES7w6SfOM4cEcfqQFaXXIPGlnoOpY6AyHJA==", null, false, "", false, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_TopicName_AuthorId",
                table: "TopicName",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK__Answer__AuthorId__1AD3FDA4",
                table: "Answer",
                column: "AuthorId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__AnswerVot__UserI__1EA48E88",
                table: "AnswerVote",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Course__UserId__787EE5A0",
                table: "Course",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CourseCom__UserI__797309D9",
                table: "CourseComment",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CourseRat__UserI__7E37BEF6",
                table: "CourseRating",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CourseSav__UserI__03F0984C",
                table: "CourseSave",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__CourseSub__UserI__7B5B524B",
                table: "CourseSubComment",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Document__UserId__0A9D95DB",
                table: "Document",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__DocumentS__UserI__0B91BA14",
                table: "DocumentSave",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__ExerciseD__UserI__01142BA1",
                table: "ExerciseDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__NotificationDetail__UserId__1EA48E88",
                table: "NotificationDetails",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Post__TopicId__76969D2E",
                table: "Post",
                column: "TopicId",
                principalTable: "TopicName",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Post__UserId__778AC167",
                table: "Post",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostComme__UserI__10566F31",
                table: "PostComment",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostLike__UserId__14270015",
                table: "PostLike",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostRepor__UserI__160F4887",
                table: "PostReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostSave__UserId__09A971A2",
                table: "PostSave",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__PostSubCo__UserI__123EB7A3",
                table: "PostSubComment",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Question__Author__18EBB532",
                table: "Question",
                column: "AuthorId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionL__UserI__208CD6FA",
                table: "QuestionLike",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionR__UserI__236943A5",
                table: "QuestionReportDetail",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__QuestionS__UserI__245D67DE",
                table: "QuestionSave",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Search__UserId__17F790F9",
                table: "Search",
                column: "UserId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__SubAnswer__Autho__1CBC4616",
                table: "SubAnswer",
                column: "AuthorId",
                principalTable: "UserShort",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__TopicDeta__Topic__06CD04F7",
                table: "TopicDetail",
                column: "TopicId",
                principalTable: "TopicName",
                principalColumn: "Id");
        }
    }
}
