using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ReplyL
{
    public class EditPostReplyModel                         // EditPostReplyModel
    {
        public int Id { get; set; }                         // id відповіді
        [Required]
        public string Message { get; set; }                 // message відповідь
        public DateTime Created { get; set; }               // date відповіді

        public virtual Post Post { get; set; }              // Пост

    }
}
