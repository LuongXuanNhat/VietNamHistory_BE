using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VNH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
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
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    PubDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
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
                name: "PostReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReport", x => x.Id);
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
                name: "UserShort",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    NumberConfirm = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        principalTable: "UserShort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PubDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Confirm = table.Column<bool>(type: "bit", nullable: true),
                    MostConfirm = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Answer__AuthorId__1AD3FDA4",
                        column: x => x.AuthorId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Course__UserId__787EE5A0",
                        column: x => x.UserId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UrlImage = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FileName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Document__UserId__0A9D95DB",
                        column: x => x.UserId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
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
                        principalTable: "UserShort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewNumber = table.Column<int>(type: "int", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PubDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuestionTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Question__Author__18EBB532",
                        column: x => x.AuthorId,
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TopicName",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        principalTable: "UserShort",
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
                    PubDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SubAnswer__Autho__1CBC4616",
                        column: x => x.AuthorId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SubAnswer__PreAn__1BC821DD",
                        column: x => x.PreAnswerId,
                        principalTable: "Answer",
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
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
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
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionReportDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReportDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__QuestionR__Quest__2180FB33",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__QuestionR__Quest__22751F6C",
                        column: x => x.QuestionReportId,
                        principalTable: "QuestionReport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__QuestionR__UserI__236943A5",
                        column: x => x.UserId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        column: x => x.Id,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Post__TopicId__76969D2E",
                        column: x => x.TopicId,
                        principalTable: "TopicName",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Post__UserId__778AC167",
                        column: x => x.UserId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TopicDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TopicDeta__TagId__07C12930",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__TopicDeta__Topic__06CD04F7",
                        column: x => x.TopicId,
                        principalTable: "TopicName",
                        principalColumn: "Id");
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
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ViewNumber = table.Column<int>(type: "int", nullable: true),
                    CommentNumber = table.Column<int>(type: "int", nullable: true),
                    LikeNumber = table.Column<int>(type: "int", nullable: true),
                    SaveNumber = table.Column<int>(type: "int", nullable: true)
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
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostReportDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
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
                        principalTable: "PostReport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PostRepor__UserI__160F4887",
                        column: x => x.UserId,
                        principalTable: "UserShort",
                        principalColumn: "Id");
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
                        principalTable: "UserShort",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Answer2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Answer3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Answer4 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RightAnswer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Quiz__Id__29221CFB",
                        column: x => x.Id,
                        principalTable: "Exercise",
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
                        principalTable: "UserShort",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5d4e4081-91f8-4fc0-b8eb-9860b7849604"), "f328002f-15b8-4eb4-8453-6243af11b0dd", "student", "student" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), "12f78230-a4c5-478d-8b4b-9a56d0f618db", "admin", "admin" },
                    { new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a"), "29343db2-d20a-4b54-9071-da021f00f9cb", "teacher", "teacher" }
                });

            migrationBuilder.InsertData(
                table: "UserShort",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Fullname", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NumberConfirm", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d1f771da-b318-42f8-a003-5a15614216f5"), 0, "14d18772-730b-41ca-bc3f-80ccbc7ce4da", new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Local), "admin@gmail.com", true, "Lương Xuân Nhất", 0, false, null, "onionwebdev@gmail.com", "admin", null, "AQAAAAEAACcQAAAAEHmqCu6W5TT0vGKr+9qbekcax+FmUEzQP1zUtUMjLmcJxCjEMm5RGAonoiYIjYFj7Q==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("d1f771da-b318-42f8-a003-5a15614216f5") });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AuthorId",
                table: "Answer",
                column: "AuthorId");

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
                name: "IX_PostDetail_PostId",
                table: "PostDetail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostDetail_UserId",
                table: "PostDetail",
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
                name: "IX_QuestionReportDetail_QuestionReportId",
                table: "QuestionReportDetail",
                column: "QuestionReportId");

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
                name: "IX_QuestionTag_TagId",
                table: "QuestionTag",
                column: "TagId");

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
                table: "TopicName",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicDetail_TagId",
                table: "TopicDetail",
                column: "TagId");

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
                name: "ExerciseDetail");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "NotificationDetails");

            migrationBuilder.DropTable(
                name: "PostDetail");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "PostReportDetail");

            migrationBuilder.DropTable(
                name: "PostSave");

            migrationBuilder.DropTable(
                name: "PostSubComment");

            migrationBuilder.DropTable(
                name: "QuestionLike");

            migrationBuilder.DropTable(
                name: "QuestionReportDetail");

            migrationBuilder.DropTable(
                name: "QuestionSave");

            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.DropTable(
                name: "Quiz");

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
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PostReport");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "QuestionReport");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "TopicName");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "UserShort");
        }
    }
}
