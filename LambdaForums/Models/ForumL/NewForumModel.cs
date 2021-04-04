using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ForumL
{
    public class NewForumModel                       // клас NewForumModel
    {
        public int Id { get; set; }                  // Id
        [Required]
        public string Title { get; set; }            // Назва
        [Required]
        public string Description { get; set; }      // Опис
        public string ImageUrl { get; set; }         // шлях картинки / (зображення , фото)
        public DateTime Created { get; set; }        // дата створення (оновлення) 

        public IFormFile ImageUpload { get; set; }   // Завантаження зображення

    }
}
