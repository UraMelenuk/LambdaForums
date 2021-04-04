using System;
using System.Collections.Generic;

namespace LambdaForums.Models
{
    public class Forum                                // Клас Forum
    {
        public int Id { get; set; }                   // Id
        public string Title { get; set; }             // Заголовок
        public string Description { get; set; }       // Опис
        public DateTime Created { get; set; }         // Дата створення
        public string ImageUrl { get; set; }          // Url адреса зображення

        public virtual IEnumerable<Post> Posts { get; set; }           // Колекція постів

    }
}
