using System;

namespace LambdaForums.Models
{
    public class PostReply                              // клас PostReply відповіді
    {
        public int Id { get; set; }                     // Id
        public string Content { get; set; }             // Опис / Зміст
        public DateTime Created { get; set; }           // Дата створення

        public virtual ApplicationUser User { get; set; }     // Користувач
        public virtual Post Post { get; set; }                // Пост

    }
}
