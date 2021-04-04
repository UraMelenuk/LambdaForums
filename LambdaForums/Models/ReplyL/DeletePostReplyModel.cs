using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ReplyL
{
    public class DeletePostReplyModel                    // Клас DeletePostReplyModel
    {
        public int Id { get; set; }                      // Id
        public string Content { get; set; }              // Опис / Зміст
        public DateTime Created { get; set; }            // Дата створення


        public virtual ApplicationUser User { get; set; }     // Користувач
        public virtual Post Post { get; set; }                // Пост
    }
}
