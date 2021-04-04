using LambdaForums.Models.ForumL;

namespace LambdaForums.Models.PostL
{
    public class PostListingModel                           // Клас PostListingModel
    {
        public int Id { get; set; }                         // id
        public string Title { get; set; }                   // Заголовок / Назва
        public string AuthorName { get; set; }              // Ім'я Автора публікації
        public int AuthorRating { get; set; }               // Рейтинг Автора публікації
        public string AuthorId { get; set; }                // Id Автора
        public string DatePosted { get; set; }              // дата

        public int ForumId { get; set; }                    // id форум
        public string ForumName { get; set; }               // назва форума
        public string ForumImageUrl { get; set; }           // шлях картинки форума

        public ForumListingModel Forum { get; set; }        // Екземпляр моделі переліку форм що відповідає кожному з публікацій
        public int RepliesCount { get; set; }               // Лічильник . (Кількість відповідей)



    }
}
