using LambdaForums.Models.PostL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.SearchL
{
    public class SearchResultModel                       // Клас SearchResultModel
    {
        public IEnumerable<PostListingModel> Posts { get; set; }   // колекція постів нашої моделі
        public string SearchQuery { get; set; }                    // пошук 
        public bool EmptySearchResults { get; set; }               // порожні результати пошуку
    }
}
