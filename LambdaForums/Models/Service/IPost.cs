using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public interface IPost                           // Інтерфейс Post
    {
        Task Add(Post post);                                                 // Add додати пост
        Task Delete(int id);                                                 // Delete видалити пост і відповіді
        Task EditPostContent(int id, string title, string content);          // EditPostContent редагувати пост по id (Заголовок , Зміст)
        Task AddReply(PostReply reply);                                      // AddReply додати відповідь

        Post GetById(int id);                                                // GetById повернути пост по Id

        int GetReplyCount(int id);                                           // GetReplyCount кількість відповідей
        string GetForumImageUrl(int id);                                     // GetForumImageUrl повернути зображення 

        IEnumerable<Post> GetAll();                                          // GetAll повернути всі пости
        IEnumerable<Post> GetFilteredPosts(string searchQuery);              // GetFilteredPosts фільтрування постів (Пошук)
        IEnumerable<Post> GetLatestPosts(int forumId);                       // GetLatestPosts повернути пости по id форуму
        

    }
}
