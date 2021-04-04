using LambdaForums.Models.PostL;
using System.Collections.Generic;

namespace LambdaForums.Models.HomeL
{
    public class HomeIndexModel                                   // Клас HomeIndexModel
    {
        public string SearchQuery { get; set; }                   // Властивість пошуку  
        public IEnumerable<PostListingModel> LatestPosts { get; set; }   // Колекція останніх публікація
    }
}
