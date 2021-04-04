using LambdaForums.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public class PostService : IPost                                       // PostService наслідує інтерфейс IPost
    {
        private readonly ApplicationDbContext _context;                    // Екземпляр db

        public PostService(ApplicationDbContext context)                   // Конструктор з параметрами
        {
            _context = context;
        }

        public async Task Add(Post post)                                   // асинхронний потік Add (додати пост)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)                        // асинхронний потік AddReply (додати відповідь)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)                                   // асинхронний потік Delete (видалити пост)
        {
            var post = GetById(id);
            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task EditPostContent(int id, string title, string content)   // асинхронний потік EditPostContent (редагувати пост по id)
        {
            var post = GetById(id);
            post.Title = title;
            post.Content = content;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()                                  // GetAll повернути всі пости
        {
            var posts = _context.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User);
            return posts;
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)      // Пошук в постах  
        {
            var query = searchQuery.ToLower();

            return _context.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .Where(post => post.Title.ToLower().Contains(query)
                || post.Content.ToLower().Contains(query));
        }


        public Post GetById(int id)                                        // повернути пости по id
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User)
                .FirstOrDefault();
        }

        public string GetForumImageUrl(int id)                             // повернути зображення форума
        {
            var post = GetById(id);
            return post.Forum.ImageUrl;
        }

        public IEnumerable<Post> GetLatestPosts(int count)                 // повернути останні пости на форумі
        {
            var allPosts = GetAll().OrderByDescending(post => post.Created);
            return allPosts.Take(count);
        }


        public int GetReplyCount(int id)                                   // повернути кількість відповідей
        {
            return GetById(id).Replies.Count();
        }
    }
}
