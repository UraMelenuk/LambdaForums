using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LambdaForums.Models;
using System;

namespace LambdaForums.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // db data base     база данних  . таблиці
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }   // Users
        public DbSet<Forum> Forums { get; set; }                       // Forums
        public DbSet<Post> Posts { get; set; }                         // Posts
        public DbSet<PostReply> PostReplies { get; set; }              // PostReplies

        internal string AsDgml()
        {
            throw new NotImplementedException();
        }
    }
}
