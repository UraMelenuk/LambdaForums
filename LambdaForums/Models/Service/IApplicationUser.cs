using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public interface IApplicationUser                                 // інтерфейс IApplicationUser
    {
        ApplicationUser GetById(string id);                           // повернути id користувача
        IEnumerable<ApplicationUser> GetAll();                        // колекція повернути всіх користувачів

        Task Add(ApplicationUser user);                               // Добавити користувача
        Task Deactivate(ApplicationUser user);                        // Активний чи не активний користувач
        Task SetProfileImage(string id, Uri uri);                     // зображення користувача
        Task IncrementRating(string id);                              // Збільшення рейтингу
        Task BumpRating(string userId, Type type);                    // Збільшення рейтигну за пост +3 чи відповідь +2

    }
}
