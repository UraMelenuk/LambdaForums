using System;
using System.ComponentModel.DataAnnotations;

namespace LambdaForums.Models.ReplyL
{
    public class PostReplyModel                      // Клас PostReplyModel
    {
        public int Id { get; set; }                  // Id

        public string AuthorId { get; set; }         // Id автора
        public string AuthorName { get; set; }       // Назва автора
        public int AuthorRating { get; set; }        // Рейтинг автора
        public string AuthorImageUrl { get; set; }   // Url адреса зображення автора
        public bool IsAuthorAdmin { get; set; }      // Чи є Адміном

        public DateTime Created { get; set; }        // Дата створення
        [Required]
        public string ReplyContent { get; set; }     // Відповідь

        public int PostId { get; set; }              // PostId
        public string PostTitle { get; set; }        // PostTitle
        public string PostContent { get; set; }      // PostContent


        public int ForumId { get; set; }             // ForumId 
        public string ForumName { get; set; }        // Forum Name
        public string ForumImageUrl { get; set; }    // Forum Image Url


    }
}
