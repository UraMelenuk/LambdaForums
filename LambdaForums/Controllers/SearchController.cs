using System.Linq;
using LambdaForums.Models;
using LambdaForums.Models.ForumL;
using LambdaForums.Models.PostL;
using LambdaForums.Models.SearchL;
using LambdaForums.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    public class SearchController : Controller              // Search Controller
    {
        private readonly IPost _postService;                // Екземпляр нашого інтерфейса IPost

        public SearchController(IPost postService)          // Конструктор з параметрами IPost
        {
            _postService = postService;
        }
        
        // Result
        public IActionResult Results(string searchQuery)    // Results пошук постів
        {
            var posts = _postService.GetFilteredPosts(searchQuery);
            var areNoResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());

            var postListing = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model = new SearchResultModel
            {
                Posts = postListing,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults
            };

            return View(model);
        }

        // Search
        [HttpPost]
        public IActionResult Search(string searchQuery)     // Search
        {
            return RedirectToAction("Results", new { searchQuery });
        }

        // BuildForumListing
        private ForumListingModel BuildForumListing(Post post)              // Метод побудови списку форуму
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl,
                Description = forum.Description
            };
        }
    }
}