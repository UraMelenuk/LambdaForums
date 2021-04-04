using LambdaForums.Models.PostL;
using System.Collections.Generic;

namespace LambdaForums.Models.ForumL
{
    public class ForumListingModel                 // Клас ForumListingModel  . Модель перегляду
    {
        public int Id { get; set; }                // Id
        public string Name { get; set; }           // Назва
        public string Description { get; set; }    // Опис
        public string ImageUrl { get; set; }       // шлях картинки / (зображення , фото)

        public int NumberOfPosts { get; set; }     // кількість постів
        public int NumberOfUsers { get; set; }     // кількість користувачів
        public bool HasRecentPosts { get; set; }   // не давня публікація

        public PostListingModel Latest { get; set; }  // PostListingModel (останні публікації)
        public IEnumerable<PostListingModel> AllPosts { get; set; }

    }
}
