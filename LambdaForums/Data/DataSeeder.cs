using LambdaForums.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Data
{
    public class DataSeeder                               // Клас DataSeeder
    {
        private ApplicationDbContext _context;            // силка на db

        public DataSeeder(ApplicationDbContext context)   // Конструктор з параметрами
        {
            _context = context;
        }

        public Task SeedSuperUser()                  // асинхронний потік метод супер юзер (Admin)
        {
            var roleStore = new RoleStore<IdentityRole>(_context);             // role
            var userStore = new UserStore<ApplicationUser>(_context);          // user

            var user = new ApplicationUser
            {
                UserName = "lambdaforumadmin@gmail.com",
                NormalizedUserName = "LAMBDAFORUMADMIN@GMAIL.COM",
                Email = "lambdaforumadmin@gmail.com",
                NormalizedEmail = "LAMBDAFORUMADMIN@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                MemberSince = DateTime.Now
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hasher.HashPassword(user, "!LambdaForumAdmin1");       //( юзер адмін  ,  пароль)
            user.PasswordHash = hashedPassword;


            var hasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");

            if (!hasAdminRole)
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                });
            }

            var hasSuperUser = _context.Users.Any(u => u.NormalizedUserName == user.UserName);

            if(!hasSuperUser)
            {
                userStore.CreateAsync(user);
                userStore.AddToRoleAsync(user,"Admin");
            }

            _context.SaveChangesAsync();

            return Task.CompletedTask;
        } 
    }
}
