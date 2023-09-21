﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Presenters.DbContext;

namespace VNH.Infrastructure.Presenters.Migrations
{
    public partial class VietNamHistoryContext : IdentityDbContext<User, Role, Guid>
    {
        public VietNamHistoryContext()
        {
        }

        public VietNamHistoryContext(DbContextOptions<VietNamHistoryContext> options)
            : base(options)
        {
        }

        #region Configuration DbSet<Object>
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationDetail> NotificationDetails { get; set; }
        public virtual DbSet<AnswerVote> AnswerVotes { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseComment> CourseComments { get; set; }
        public virtual DbSet<CourseRating> CourseRatings { get; set; }
        public virtual DbSet<CourseSave> CourseSaves { get; set; }
        public virtual DbSet<CourseSubComment> CourseSubComments { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentSave> DocumentSaves { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<ExerciseDetail> ExerciseDetails { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<PostDetail> PostDetails { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }
        public virtual DbSet<PostReport> PostReports { get; set; }
        public virtual DbSet<PostReportDetail> PostReportDetails { get; set; }
        public virtual DbSet<PostSave> PostSaves { get; set; }
        public virtual DbSet<PostSubComment> PostSubComments { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionLike> QuestionLikes { get; set; }
        public virtual DbSet<QuestionReport> QuestionReports { get; set; }
        public virtual DbSet<QuestionReportDetail> QuestionReportDetails { get; set; }
        public virtual DbSet<QuestionSave> QuestionSaves { get; set; }
        public virtual DbSet<QuestionTag> QuestionTags { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Search> Searches { get; set; }
        public virtual DbSet<SubAnswer> SubAnswers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TopicDetail> TopicDetails { get; set; }
        public virtual DbSet<User> User { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VietNamHistory;Integrated Security=True;Encrypt=true;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<NotificationDetail>(entity =>
            {
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.HasOne(p => p.Notification)
                    .WithMany(x => x.NotificationDetails)
                    .HasForeignKey(a => a.NotificationId)
                    .HasConstraintName("FK__NotificationDetail__NotificationId__1EQ48E88"); ;

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__NotificationDetail__UserId__1EA48E88");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Answer__AuthorId__1AD3FDA4");
            });

            modelBuilder.Entity<AnswerVote>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.AnswerVotes)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK__AnswerVot__Answe__1DB06A4F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AnswerVotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__AnswerVot__UserI__1EA48E88");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Course__UserId__787EE5A0");
            });

            modelBuilder.Entity<CourseComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseComments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseCom__Cours__7A672E12");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CourseCom__UserI__797309D9");
            });

            modelBuilder.Entity<CourseRating>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseRatings)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseRat__Cours__7D439ABD");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseRatings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CourseRat__UserI__7E37BEF6");
            });

            modelBuilder.Entity<CourseSave>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseSaves)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseSav__Cours__04E4BC85");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseSaves)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CourseSav__UserI__03F0984C");
            });

            modelBuilder.Entity<CourseSubComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.PreComment)
                    .WithMany(p => p.CourseSubComments)
                    .HasForeignKey(d => d.PreCommentId)
                    .HasConstraintName("FK__CourseSub__PreCo__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseSubComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CourseSub__UserI__7B5B524B");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Document__UserId__0A9D95DB");
            });

            modelBuilder.Entity<DocumentSave>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentSaves)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK__DocumentS__Docum__0C85DE4D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentSaves)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__DocumentS__UserI__0B91BA14");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Exercise)
                    .HasForeignKey<Exercise>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Exercise__Id__282DF8C2");
            });

            modelBuilder.Entity<ExerciseDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseDetails)
                    .HasForeignKey(d => d.ExerciseId)
                    .HasConstraintName("FK__ExerciseD__Exerc__02084FDA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExerciseDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ExerciseD__UserI__01142BA1");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Lesson__CourseId__7F2BE32F");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__Post__TopicId__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Post__UserId__778AC167");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostComme__PostI__0F624AF8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostComme__UserI__10566F31");
            });

            modelBuilder.Entity<PostDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostDetails)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostDetai__PostI__0D7A0286");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostDetai__UserI__0E6E26BF");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostLike__PostId__1332DBDC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostLike__UserId__14270015");
            });

            modelBuilder.Entity<PostReport>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<PostReportDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostReportDetails)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostRepor__PostI__17036CC0");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.PostReportDetails)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK__PostRepor__Repor__151B244E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostReportDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostRepor__UserI__160F4887");
            });

            modelBuilder.Entity<PostSave>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostSaves)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostSave__PostId__08B54D69");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostSaves)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostSave__UserId__09A971A2");
            });

            modelBuilder.Entity<PostSubComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.PreComment)
                    .WithMany(p => p.PostSubComments)
                    .HasForeignKey(d => d.PreCommentId)
                    .HasConstraintName("FK__PostSubCo__PreCo__114A936A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostSubComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostSubCo__UserI__123EB7A3");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Question__Author__18EBB532");
            });

            modelBuilder.Entity<QuestionLike>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionLikes)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__QuestionL__Quest__1F98B2C1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuestionLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__QuestionL__UserI__208CD6FA");
            });

            modelBuilder.Entity<QuestionReport>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<QuestionReportDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionReportDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__QuestionR__Quest__2180FB33");

                entity.HasOne(d => d.QuestionReport)
                    .WithMany(p => p.QuestionReportDetails)
                    .HasForeignKey(d => d.QuestionReportId)
                    .HasConstraintName("FK__QuestionR__Quest__22751F6C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuestionReportDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__QuestionR__UserI__236943A5");
            });

            modelBuilder.Entity<QuestionSave>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionSaves)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__QuestionS__Quest__25518C17");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuestionSaves)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__QuestionS__UserI__245D67DE");
            });

            modelBuilder.Entity<QuestionTag>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.QuestionTag)
                    .HasForeignKey<QuestionTag>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionTag__Id__2739D489");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.QuestionTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__QuestionT__TagId__2645B050");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Quiz)
                    .HasForeignKey<Quiz>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Quiz__Id__29221CFB");
            });

            modelBuilder.Entity<Search>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Searches)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Search__UserId__17F790F9");
            });

            modelBuilder.Entity<SubAnswer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.SubAnswers)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__SubAnswer__Autho__1CBC4616");

                entity.HasOne(d => d.PreAnswer)
                    .WithMany(p => p.SubAnswers)
                    .HasForeignKey(d => d.PreAnswerId)
                    .HasConstraintName("FK__SubAnswer__PreAn__1BC821DD");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Topic__AuthorId__05D8E0BE");
            });

            modelBuilder.Entity<TopicDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TopicDetails)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__TopicDeta__TagId__07C12930");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicDetails)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__TopicDeta__Topic__06CD04F7");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

          
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            OnModelCreatingPartial(modelBuilder);
            new SeedingData(modelBuilder).Seed();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}