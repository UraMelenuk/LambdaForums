using LambdaForums.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public class ForumService : IForum                         // Клас ForumService наслідуе інтерфейс IForum
    {
        private readonly ApplicationDbContext _context;        // Екземпляр нашого db context
        private readonly IPost _postService;                   // Екземпляр нашого інтерфейса IPost

        public ForumService(ApplicationDbContext context , IPost postService)   // консруктор з парамететрами
        {
            _context = context;
            _postService = postService;
        }

        // Create
        public async Task Create(Forum forum)                  // асинхронний метод створити форум
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task Delete(int id)                       // асинхронний метод видалити форум id
        {
            var forum = GetById(id);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        // GetActiveUser
        public IEnumerable<ApplicationUser> GetActiveUsers(int id)   // активні користувачі
        {
            var posts = GetById(id).Posts;

            if (posts != null || !posts.Any())
            {
                var postUsers = posts.Select(p => p.User);
                var replyUsers = posts.SelectMany(p => p.Replies).Select(r => r.User);

                return postUsers.Union(replyUsers).Distinct();
            }

            return new List<ApplicationUser>();
        }

        // GetAll
        public IEnumerable<Forum> GetAll()                     // повертаємо всі форуми (пости)
        {
            return _context.Forums.Include(forum => forum.Posts);
        }

        // GetById
        public Forum GetById(int id)                           // повернемо по id
        {
            var forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts).ThenInclude(f => f.User)
                .Include(f => f.Posts).ThenInclude(f => f.Replies).ThenInclude(f => f.User)
                .Include(f => f.Posts).ThenInclude(p => p.Forum)
                .FirstOrDefault();

            if (forum.Posts == null)
            {
                forum.Posts = new List<Post>();
            }

            return forum;
        }

        // GetFilteredPosts
        public IEnumerable<Post> GetFilteredPosts(string searchQuery)   // пошук в постах
        {
            return _postService.GetFilteredPosts(searchQuery);
        }

        // GetFilteredPosts
        public IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery)   // Глобальний пошук у форумах
        {
            if (forumId == 0) return _postService.GetFilteredPosts(searchQuery);

            var forum = GetById(forumId);

            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post
                    => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        // GetLatestPost
        public Post GetLatestPost(int forumId)              // останні публікації пости 
        {
            var posts = GetById(forumId).Posts;

            if (posts != null)
            {
                return GetById(forumId).Posts
                    .OrderByDescending(post => post.Created)
                    .FirstOrDefault();
            }

            return new Post();
        }

        // HasRecentPost
        public bool HasRecentPost(int id)                   // активні повідомлення за 12 год
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);

            return GetById(id).Posts.Any(post => post.Created > window);
        }

        // Update
        public async Task Update(int id , string newTitle, string newDescription, string newImageUrl)   // асинхронний метод оновити форум (Заголовок і Опис)
        {
            var forum = GetById(id);
            forum.Title = newTitle;
            forum.Description = newDescription;
            forum.ImageUrl = newImageUrl;
            forum.Created = DateTime.Now;

            _context.Update(forum);
            await _context.SaveChangesAsync();
        }

    }
}
