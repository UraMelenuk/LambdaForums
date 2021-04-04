using System.Collections.Generic;

namespace LambdaForums.Models.ForumL
{
    public class ForumIndexModel                        // Клас ForumIndexModel (обгортає колекцію ношої моделі з переліком форм)
    {
        public IEnumerable<ForumListingModel> ForumList { get; set; }    // Колекція

    }
}
