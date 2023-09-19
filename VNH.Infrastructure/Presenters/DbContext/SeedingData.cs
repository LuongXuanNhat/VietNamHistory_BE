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
            //      ADMINISTRATOR
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
                new Role {
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
        }
    }
}
