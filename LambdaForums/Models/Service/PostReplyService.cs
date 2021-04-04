using LambdaForums.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public class PostReplyService : IPostReply                   // клас PostReplyService наслідує інтерфейс IPostReply
    {
        private readonly ApplicationDbContext _context;          // db context

        public PostReplyService(ApplicationDbContext context)    // конструктор з параметрами
        {
            _context = context;
        }

        public PostReply GetById(int id)                         // Повернути по id відповідь
        {

            return _context.PostReplies
                .Include(r => r.Post).ThenInclude(post => post.Forum)
                .Include(r => r.Post).ThenInclude(post => post.User).First(r => r.Id == id);
        }

        public async Task Delete(int id)                         // асинхронний метод видалити відповідь
        {
            var reply = GetById(id);
            _context.Remove(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, string message)         // асинхронний метод оновити відповідь
        {
            var reply = GetById(id);
            reply.Content = message;
            reply.Created = DateTime.Now;

            await _context.SaveChangesAsync();
            _context.Update(reply);
            await _context.SaveChangesAsync();

        }
    }
}
