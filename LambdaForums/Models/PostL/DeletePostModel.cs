using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.PostL
{
    public class DeletePostModel                              // клас DeletePostModel 
    {
        /*
        public int PostId { get; set; }
        public string PostAuthor { get; set; }
        public string PostContent { get; set; }
        */
        
        public int PostId { get; set; }                    // Id
        public string Title { get; set; }                  // Заголовок
        public string Content { get; set; }                // Опис / Зміст
        public DateTime Created { get; set; }              // Дата створеня

        public virtual ApplicationUser User { get; set; }    // Користувач
        public virtual Forum Forum { get; set; }             // Форум

        public virtual IEnumerable<PostReply> Replies { get; set; }   // Колекція відповідей
        
    }
}
