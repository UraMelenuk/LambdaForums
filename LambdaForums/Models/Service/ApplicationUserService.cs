using LambdaForums.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public class ApplicationUserService : IApplicationUser              // ApplicationUserService наслідує інтерфейс IApplicationUser
    {
        private readonly ApplicationDbContext _context;                 // Екземпляр нашого db context

        public ApplicationUserService(ApplicationDbContext context)     // Конструктор з параметром
        {
            _context = context;
        }

        // GetById
        public ApplicationUser GetById(string id)                       // повернути користувача по id
        {
            return _context.ApplicationUsers.FirstOrDefault(user => user.Id == id);
        }

        // GetAll
        public IEnumerable<ApplicationUser> GetAll()                    // колекція користувачів      
        {
            return _context.ApplicationUsers;
        }

        // Add
        public async Task Add(ApplicationUser user)                     // асинхронний метод додбавити користувача
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        // Deactivate
        public async Task Deactivate(ApplicationUser user)              // асинхронний метод активний чи не активний користувач 
        {
            user.IsActive = false;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        // SetProfileImage
        public async Task SetProfileImage(string id, Uri uri)           // асинхронний метод завантаження зображення користувача
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        // IncrementRating
        public async Task IncrementRating(string id)                    // асинхронний метод збільшити рейтинг користувачу на 1
        {
            var user = GetById(id);
            user.Rating += 1;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        // BumpRating
        public async Task BumpRating(string userId, Type type)           // збільшення рейтинга користувача (пост чи відповідь)
        {
            var user = GetById(userId);
            var increment = GetIncrement(type);
            user.Rating += increment;
            await _context.SaveChangesAsync();
        }
        
        // GetIncrement
        private static int GetIncrement(Type type)                       // статичний метод отримати рейтинг за пост чи відповідь
        {
            var bump = 0;

            if (type == typeof(Post))
            {
                bump = 3;
            }

            if (type == typeof(PostReply))
            {
                bump = 2;
            }

            return bump;
        }
    }
}
