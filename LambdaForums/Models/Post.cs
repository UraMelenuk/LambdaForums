using System;
using System.Collections.Generic;

namespace LambdaForums.Models
{
    public class Post                                  // Клас Post
    {
        public int Id { get; set; }                    // Id
        public string Title { get; set; }              // Заголовок
        public string Content { get; set; }            // Опис / Зміст
        public DateTime Created { get; set; }          // Дата створеня

        public virtual ApplicationUser User { get; set; }    // Користувач
        public virtual Forum Forum { get; set; }             // Форум

        public virtual IEnumerable<PostReply> Replies { get; set; }   // Колекція відповідей
    }
}
