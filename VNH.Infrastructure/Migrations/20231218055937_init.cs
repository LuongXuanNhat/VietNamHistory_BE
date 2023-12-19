using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", maxLength: 3145728, nullable: true),
                    NumberConfirm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });

                    table.ForeignKey(
                      name: "FK_AppUserRoles_Roles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "Roles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_AppUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Course__UserId__787EE5A0",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Document__UserId__0A9D95DB",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    WorkTime = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoise", x => x.Id);
                    table.ForeignKey(
                        name: "FK__MultipleChoice__UserId__06CD04F7",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK__NotificationDetail__NotificationId__1EQ48E88",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__NotificationDetail__UserId__1EA48E88",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewNumber = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Question__Author__18EBB532",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Search",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Search", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Search__UserId__17F790F9",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Topic__AuthorId__05D8E0BE",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CourseCom__Cours__7A672E12",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CourseCom__UserI__797309D9",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseRating",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CourseRat__Cours__7D439ABD",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CourseRat__UserI__7E37BEF6",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseSave",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSave", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CourseSav__Cours__04E4BC85",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CourseSav__UserI__03F0984C",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlVideo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Lesson__CourseId__7F2BE32F",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentSave",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSave", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DocumentS__Docum__0C85DE4D",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__DocumentS__UserI__0B91BA14",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MultipleChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scores = table.Column<int>(type: "int", nullable: false),
                    CompletionTime = table.Column<int>(type: "int", nullable: false),
                    StarDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamHistory_MultipleChoise_MultipleChoiceId",
                        column: x => x.MultipleChoiceId,
                        principalTable: "MultipleChoise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MultipleChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK__MultipleChoice__Id__07C12930",
                        column: x => x.MultipleChoiceId,
                        principalTable: "MultipleChoise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Confirm = table.Column<bool>(type: "bit", nullable: true),
                    MostConfirm = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Answer__AuthorId__1AD3FDA4",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Answer__QuestionId__1AD3FVA4",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuestionL__Quest__1F98B2C1",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__QuestionL__UserI__208CD6FA",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionReportDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReportDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuestionR__Quest__2180FB33",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__QuestionR__Quest__22751F6C",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__QuestionR__UserI__236943A5",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionSave",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSave", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuestionS__Quest__25518C17",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__QuestionS__UserI__245D67DE",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuestionT__TagId__2645B050",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__QuestionTag__Id__2739D489",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewNumber = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Post__TopicId__76969D2E",
                        column: x => x.TopicId,
                        principalTable: "Topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Post__UserId__778AC167",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseSubComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CourseSub__PreCo__7C4F7684",
                        column: x => x.PreCommentId,
                        principalTable: "CourseComment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CourseSub__UserI__7B5B524B",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exercise__Id__282DF8C2",
                        column: x => x.Id,
                        principalTable: "Lesson",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCorrect = table.Column<bool>(type: "bit", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuizAnswers__QuizId__1AD3FVA4",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AnswerVot__Answe__1DB06A4F",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__AnswerVot__UserI__1EA48E88",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SubAnswer__Autho__1CBC4616",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SubAnswer__PreAn__1BC821DD",
                        column: x => x.PreAnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostComme__PostI__0F624AF8",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostComme__UserI__10566F31",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostLike__PostId__1332DBDC",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostLike__UserId__14270015",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostReportDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReportDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostRepor__PostI__17036CC0",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostRepor__Repor__151B244E",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__PostRepor__UserI__160F4887",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostSave",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSave", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostSave__PostId__08B54D69",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostSave__UserId__09A971A2",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostTags_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TopicDeta__TagId__07C12930",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__TopicDeta__Topic__06CD04F7",
                        column: x => x.TopicId,
                        principalTable: "Topic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TestMark = table.Column<double>(type: "float", nullable: true),
                    TestTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ExerciseD__Exerc__02084FDA",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ExerciseD__UserI__01142BA1",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostSubComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSubComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PostSubCo__PreCo__114A936A",
                        column: x => x.PreCommentId,
                        principalTable: "PostComment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostSubCo__UserI__123EB7A3",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Report",
                columns: new[] { "Id", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("25752490-4ba5-4abb-ac3b-192205cd1b6e"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1435), "Sử dụng khi bạn thấy nội dung bài đăng chứa lời lẽ xúc phạm, kỳ thị hoặc có tính chất đe doạ đến người khác.", "Nội dung xấu, xúc phạm, hay kỳ thị" },
                    { new Guid("3043c693-b3c9-453e-9876-31c943222576"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1453), "Dùng khi bạn muốn báo cáo vì nó quá nhiều thông báo hoặc quảng cáo không mong muốn.", "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn" },
                    { new Guid("349ed807-6107-436f-9a4c-9d6183fbc444"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1442), "Sử dụng khi bạn thấy nội dung chứa hình ảnh tự tử hoặc khuyến khích hành vi tự gây thương tổn.", "Chứa nội dung tự tử hoặc tự gây thương tổn" },
                    { new Guid("4a780087-9058-41c9-b84b-944d1a502010"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1449), "Sử dụng khi bạn thấy rằng nội dung chứa thông tin sai lệch, giả mạo hoặc vi phạm quy tắc về sự thật và trung thực.", "Bài đăng chứa thông tin sai lệch hoặc giả mạo" },
                    { new Guid("bab1da58-6921-44b9-837f-c58d3998497b"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1438), "Dùng khi bạn thấy nội dung chứa hình ảnh hoặc video bạo lực hoặc đội nhóm xấu, hoặc khuyến khích hành vi bạo lực.", "Chứa nội dung bạo lực hoặc đội nhóm xấu" },
                    { new Guid("c4ddb872-06c5-4779-a8a3-a55e5b2c5347"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1445), "Sử dụng khi bạn cho rằng Nội dung vi phạm quyền sở hữu trí tuệ hoặc bản quyền, chẳng hạn như sử dụng hình ảnh hoặc video mà bạn sở hữu mà không có sự cho phép.", "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ" },
                    { new Guid("d30e1353-0163-43c1-b757-7957981b0eda"), new DateTime(2023, 12, 18, 12, 59, 36, 825, DateTimeKind.Local).AddTicks(1413), " Báo cáo này được sử dụng khi người dùng chia sẻ nội dung cá nhân của bạn mà bạn cho rằng vi phạm quyền riêng tư của bạn.", "Nội dung vi phạm quy định về quyền riêng tư" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"), "f2c7ce5d-de4f-496a-bc53-129835856dc1", "student", "student" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), "b8563bb9-4b36-4a5b-864b-36b593d5d36c", "admin", "admin" },
                    { new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"), "f5fcff63-f067-448e-b72e-aefb426c558d", "teacher", "teacher" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Fullname", "Gender", "Image", "Introduction", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NumberConfirm", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), 0, "4cc11f7a-e277-40e9-8018-03bd9affb56d", new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Local), "admin@gmail.com", true, "Lương Xuân Nhất", 0, "", null, false, false, null, "onionwebdev@gmail.com", "admin", null, "AQAAAAEAACcQAAAAELthC2YX0yD3LyCfcc90BjRdfZ/u/PQV2FZWsQ2HpyTwsxIOediSiMRDm+kImP50+Q==", null, false, "", false, "admin" });

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
                    { new Guid("12f52ba9-2673-4d2c-86a8-984335cc4129"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Vương quốc Âu Lạc.", null },
                    { new Guid("19ef7fc7-5f58-4e90-a59a-3972ad2ef157"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Nguyễn", null },
                    { new Guid("25a66923-d6e8-47da-9b8b-3d5f2efa0f33"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nghệ thuật và văn hóa đương đại", null },
                    { new Guid("27fd3609-9965-4d57-99b7-4fcbb8653547"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chính sách đổi mới", null },
                    { new Guid("2f3dea3f-47d1-40aa-ad9b-25dc19fad03f"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Đời sống trong xã cổ Đông Sơn.", null },
                    { new Guid("3f447575-d3f3-415d-bc59-5a1397ff4938"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Giao lưu văn hóa quốc tế", null },
                    { new Guid("3fe8d29b-059e-477a-b7f7-a1401545a4b8"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Nhà Hậu Lê", null },
                    { new Guid("52cd4ca9-3029-40dd-bbf2-e44d726b9b2e"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ đổi mới", null },
                    { new Guid("568558af-648d-46ae-8317-ade70012f3cf"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa và Nghệ thuật", null },
                    { new Guid("73c9975b-7e5d-4a67-be4f-ea8088ef0b1a"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Văn hóa dân gian", null },
                    { new Guid("81631b70-12f2-4118-b362-16ce4f9b1b85"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Lịch sử thời kỳ thuộc địa", null },
                    { new Guid("8b2cc6db-df2a-4390-a6a8-9d40c27ec2d1"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Anh hùng", null },
                    { new Guid("95807a63-e5df-49af-bb11-0c210cff64d9"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Phong trào Duy Tân", null },
                    { new Guid("975ade78-8f23-41e2-8b7e-0885f4daa97c"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Triều đại Lý.", null },
                    { new Guid("98d9ed5f-3ba9-44ed-bd0b-f887cb34f1f8"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Pháp-Đông Dương", null },
                    { new Guid("a164d0be-812c-4a9c-ab5f-c9d4d04bb4cf"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null },
                    { new Guid("ab9bfbd4-26a1-42b0-bef6-ac239e8df4a3"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ thuộc địa Pháp", null },
                    { new Guid("be364be8-ed1c-4997-8ed5-4b0b53857cb8"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thách thức hiện đại hóa", null },
                    { new Guid("daa7b78e-80a7-4b24-92c2-34fa17d77da9"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Thời kỳ Trung đại", null },
                    { new Guid("eca76873-e0d8-4c7f-ae25-f05707e8ee35"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Những thăng trầm của triều đại Lê", null },
                    { new Guid("fe70bbdb-fa47-4bde-8b72-6a531488ac4d"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), null, "Chiến tranh Việt Nam", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AuthorId",
                table: "Answer",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerVote_AnswerId",
                table: "AnswerVote",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerVote_UserId",
                table: "AnswerVote",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_UserId",
                table: "Course",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComment_CourseId",
                table: "CourseComment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComment_UserId",
                table: "CourseComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRating_CourseId",
                table: "CourseRating",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRating_UserId",
                table: "CourseRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSave_CourseId",
                table: "CourseSave",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSave_UserId",
                table: "CourseSave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubComment_PreCommentId",
                table: "CourseSubComment",
                column: "PreCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubComment_UserId",
                table: "CourseSubComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId",
                table: "Document",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSave_DocumentId",
                table: "DocumentSave",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSave_UserId",
                table: "DocumentSave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamHistory_MultipleChoiceId",
                table: "ExamHistory",
                column: "MultipleChoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseDetail_ExerciseId",
                table: "ExerciseDetail",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseDetail_UserId",
                table: "ExerciseDetail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_CourseId",
                table: "Lesson",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoise_UserId",
                table: "MultipleChoise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDetails_NotificationId",
                table: "NotificationDetails",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDetails_UserId",
                table: "NotificationDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_TopicId",
                table: "Post",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostId",
                table: "PostComment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_UserId",
                table: "PostComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_PostId",
                table: "PostLike",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_UserId",
                table: "PostLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReportDetail_PostId",
                table: "PostReportDetail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReportDetail_ReportId",
                table: "PostReportDetail",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReportDetail_UserId",
                table: "PostReportDetail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSave_PostId",
                table: "PostSave",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSave_UserId",
                table: "PostSave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSubComment_PreCommentId",
                table: "PostSubComment",
                column: "PreCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSubComment_UserId",
                table: "PostSubComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_AuthorId",
                table: "Question",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLike_QuestionId",
                table: "QuestionLike",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLike_UserId",
                table: "QuestionLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReportDetail_QuestionId",
                table: "QuestionReportDetail",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReportDetail_ReportId",
                table: "QuestionReportDetail",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReportDetail_UserId",
                table: "QuestionReportDetail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSave_QuestionId",
                table: "QuestionSave",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSave_UserId",
                table: "QuestionSave",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_QuestionId",
                table: "QuestionTag",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagId",
                table: "QuestionTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_MultipleChoiceId",
                table: "Quiz",
                column: "MultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswer_QuizId",
                table: "QuizAnswer",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Search_UserId",
                table: "Search",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubAnswer_AuthorId",
                table: "SubAnswer",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubAnswer_PreAnswerId",
                table: "SubAnswer",
                column: "PreAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_AuthorId",
                table: "Topic",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicDetail_PostId",
                table: "TopicDetail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicDetail_TopicId",
                table: "TopicDetail",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerVote");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "CourseRating");

            migrationBuilder.DropTable(
                name: "CourseSave");

            migrationBuilder.DropTable(
                name: "CourseSubComment");

            migrationBuilder.DropTable(
                name: "DocumentSave");

            migrationBuilder.DropTable(
                name: "ExamHistory");

            migrationBuilder.DropTable(
                name: "ExerciseDetail");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "NotificationDetails");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "PostReportDetail");

            migrationBuilder.DropTable(
                name: "PostSave");

            migrationBuilder.DropTable(
                name: "PostSubComment");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropTable(
                name: "QuestionLike");

            migrationBuilder.DropTable(
                name: "QuestionReportDetail");

            migrationBuilder.DropTable(
                name: "QuestionSave");

            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.DropTable(
                name: "QuizAnswer");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Search");

            migrationBuilder.DropTable(
                name: "SubAnswer");

            migrationBuilder.DropTable(
                name: "TopicDetail");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CourseComment");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "MultipleChoise");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
