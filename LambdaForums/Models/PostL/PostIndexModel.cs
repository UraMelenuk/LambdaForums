using LambdaForums.Models.ReplyL;
using System;
using System.Collections.Generic;

namespace LambdaForums.Models.PostL
{
    public class PostIndexModel                          // Клас PostIndexModel
    {
        public int Id { get; set; }                      // Id
        public string Title { get; set; }                // Назва
        public string AuthorId { get; set; }             // Id автора
        public string AuthorName { get; set; }           // Назва автора
        public string AuthorImageUrl { get; set; }       // Url адреса зображення автора
        public int AuthorRating { get; set; }            // Рейтинг Автора
        public DateTime Date { get; set; }               // Дата створення
        public bool IsAuthorAdmin { get; set; }          // Чи є Адміном      
        public string PostContent { get; set; }          // Зміст допису
        
        public int ForumId { get; set; }                 // Форум id
        public string ForumName { get; set; }            // Форум назва

        public IEnumerable<PostReplyModel> Replies { get; set; }   // Колекція відповідей

    }
}
