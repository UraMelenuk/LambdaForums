using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ForumL
{
    public class DeleteForumModel                       // DeleteForumModel
    {
        public int ForumId { get; set; }           // Id форума
        public string Title { get; set; }          // Назва форума
        public string Description { get; set; }    // Опис форума
        public DateTime Created { get; set; }      // Дата створення
        public string ImageUrl { get; set; }       // шлях картинки / (зображення , фото) форума


        public virtual IEnumerable<Post> Posts { get; set; }          // Загальні Колекції пости
        public virtual IEnumerable<PostReply> Replies { get; set; }   // Загальні Колекції відповіді

    }
}
