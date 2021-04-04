using LambdaForums.Models.PostL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ForumL
{
    public class ForumTopicModel                      // Клас ForumTopicModel (коли ми маємо пошуковий запит ми передаєм цю модель)
    {
        public ForumListingModel Forum { get; set;}   // Проста модель переліку форм

        public IEnumerable<PostListingModel> Posts { get; set; }   // Загальні Колекції

        public string SearchQuery { get; set; }            // Пошук
    }
}
