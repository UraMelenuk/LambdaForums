using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.PostL
{
    public class EditPostModel                              // клас EditPostModel
    {
        public int Id { get; set; }                    // Id
        [Required]
        public string Title { get; set; }              // Заголовок
        [Required]
        public string Content { get; set; }            // Опис / Зміст
        public DateTime Created { get; set; }          // Дата створеня

        public virtual ApplicationUser User { get; set; }    // Користувач
        public virtual Forum Forum { get; set; }             // Форум

        public virtual IEnumerable<PostReply> Replies { get; set; }   // Колекція відповідей
    }
}
