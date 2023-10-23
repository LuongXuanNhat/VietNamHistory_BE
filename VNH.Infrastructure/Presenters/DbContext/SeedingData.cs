using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Domain;

namespace VNH.Infrastructure.Presenters.DbContext
{
    public class SeedingData
    {
        private readonly ModelBuilder modelBuilder;

        public SeedingData(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
           // ADMINISTRATOR
            var roleId = new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575");
            var roleId1 = new Guid("cfafcfcd-d796-43f4-8ac0-ead43bd2f18a");
            var roleId2 = new Guid("5D4E4081-91F8-4FC0-B8EB-9860B7849604");

            var adminId = new Guid("D1F771DA-B318-42F8-A003-5A15614216F5");

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = roleId,
                    Name = "admin",
                    NormalizedName = "admin"
                },
                new Role
                {
                    Id = roleId1,
                    Name = "teacher",
                    NormalizedName = "teacher"
                },
                new Role
                {
                    Id = roleId2,
                    Name = "student",
                    NormalizedName = "student"
                });


            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                UserName = "admin",
                Fullname = "Lương Xuân Nhất",
                Gender = Domain.Enums.Gender.male,
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "onionwebdev@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Aa@123"),
                SecurityStamp = string.Empty,
                DateOfBirth = new DateTime(2002, 03, 18, 0, 0, 0, DateTimeKind.Local),

            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

            modelBuilder.Entity<Report>().HasData(
            new Report
            {
                Id = new Guid("D30E1353-0163-43C1-B757-7957981B0EDA"),
                CreatedAt = DateTime.Now,
                Title = "Nội dung vi phạm quy định về quyền riêng tư",
                Description = " Báo cáo này được sử dụng khi người dùng chia sẻ nội dung cá nhân của bạn mà bạn cho rằng vi phạm quyền riêng tư của bạn."
            },
            new Report
            {
                Id = new Guid("25752490-4BA5-4ABB-AC3B-192205CD1B6E"),
                CreatedAt = DateTime.Now,
                Title = "Nội dung xấu, xúc phạm, hay kỳ thị",
                Description = "Sử dụng khi bạn thấy nội dung bài đăng chứa lời lẽ xúc phạm, kỳ thị hoặc có tính chất đe doạ đến người khác."
            },
            new Report
            {
                Id = new Guid("BAB1DA58-6921-44B9-837F-C58D3998497B"),
                CreatedAt = DateTime.Now,
                Title = "Chứa nội dung bạo lực hoặc đội nhóm xấu",
                Description = "Dùng khi bạn thấy nội dung chứa hình ảnh hoặc video bạo lực hoặc đội nhóm xấu, hoặc khuyến khích hành vi bạo lực."
            },
            new Report
            {
                Id = new Guid("349ED807-6107-436F-9A4C-9D6183FBC444"),
                CreatedAt = DateTime.Now,
                Title = "Chứa nội dung tự tử hoặc tự gây thương tổn",
                Description = "Sử dụng khi bạn thấy nội dung chứa hình ảnh tự tử hoặc khuyến khích hành vi tự gây thương tổn."
            },
            new Report
            {
                Id = new Guid("C4DDB872-06C5-4779-A8A3-A55E5B2C5347"),
                CreatedAt = DateTime.Now,
                Title = "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ",
                Description = "Sử dụng khi bạn cho rằng Nội dung vi phạm quyền sở hữu trí tuệ hoặc bản quyền, chẳng hạn như sử dụng hình ảnh hoặc video mà bạn sở hữu mà không có sự cho phép."
            },
            new Report
            {
                Id = new Guid("4A780087-9058-41C9-B84B-944D1A502010"),
                CreatedAt = DateTime.Now,
                Title = "Bài đăng chứa thông tin sai lệch hoặc giả mạo",
                Description = "Sử dụng khi bạn thấy rằng nội dung chứa thông tin sai lệch, giả mạo hoặc vi phạm quy tắc về sự thật và trung thực."
            },
            new Report
            {
                Id = new Guid("3043C693-B3C9-453E-9876-31C943222576"),
                CreatedAt = DateTime.Now,
                Title = "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn",
                Description = "Dùng khi bạn muốn báo cáo vì nó quá nhiều thông báo hoặc quảng cáo không mong muốn."
            }
            );
        }
    }
}
