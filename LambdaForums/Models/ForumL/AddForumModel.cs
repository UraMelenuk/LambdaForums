using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ForumL
{
    public class AddForumModel                        // Клас AddForumModel  (створити нову модель форуму)
    {
        [Required]
        public string Title { get; set; }             // Заголовок
        [Required]
        public string Description { get; set; }       // Опис
        public string ImageUrl { get; set; }          // Url адреса зображення

        public IFormFile ImageUpload { get; set; }    // Завантаження зображення

    }
}
