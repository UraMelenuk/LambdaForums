using System;
using System.ComponentModel.DataAnnotations;

namespace LambdaForums.Models.PostL
{
    public class NewPostModel                            // Клас NewPostModel
    {
        public int Id { get; set; }                      // Id
        [Required]
        public string Title { get; set; }                // Заголовок
        public int ForumId { get; set; }                 // Id Форума
        public string ForumName { get; set; }            // Назва Форума
        public string ForumImageUrl { get; set; }        // Url адреса зображення (щоб отримати візуальний звязок)
        [Required]
        public string Content { get; set; }              // Зміст
        public DateTime Created { get; set; }            // Дата
        public string UserId { get; set; }               // Id Користувача
        public string AuthorName { get; set; }           // Ім'я Автора
        
        

    }
}
