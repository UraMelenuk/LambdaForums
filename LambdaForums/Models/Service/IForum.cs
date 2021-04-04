using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public interface IForum                                 // Інтерфейс IForum
    {
        Forum GetById(int id);                              // Повернути Id
        IEnumerable<Forum> GetAll();                        // Колекція повернути все

        Task Create(Forum forum);                                          // Створити
        Task Delete(int forumId);                                          // видалити по id
        Task Update(int Id, string newTitle, string newDescription, string newImageUrl); // оновити по заголовок , опис , зображення
        Post GetLatestPost(int forumId);                                   // повернути останні публікації по id

        IEnumerable<ApplicationUser> GetActiveUsers(int id);   // повернути активних користувачів по id
        bool HasRecentPost(int id);                            // активні повідомлення

        IEnumerable<Post> GetFilteredPosts(string searchQuery); // глобальний пошук на форумах і для пошуку в постах 
        IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery);

    }
}
