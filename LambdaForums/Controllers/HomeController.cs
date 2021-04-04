using Microsoft.AspNetCore.Mvc;
using LambdaForums.Models.HomeL;
using LambdaForums.Models.Service;
using System.Linq;
using LambdaForums.Models.PostL;

namespace LambdaForums.Controllers
{
    public class HomeController : Controller                                     // HomeController
    {
        private readonly IPost _postService;                                     // Екземпляр інтерфейса IPost

        public HomeController(IPost postService)                                 // Конструктор з параметрами
        {
            _postService = postService;
        }

        // Index
        public IActionResult Index()                                             // Index
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

        // BuildHomeIndexModel
        public HomeIndexModel BuildHomeIndexModel()                              // BuildHomeIndexModel відображати останні 10 постів на форумі
        {
            var latest = _postService.GetLatestPosts(10);

            var posts = latest.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = _postService.GetReplyCount(post.Id),
                ForumName = post.Forum.Title,
                ForumImageUrl = _postService.GetForumImageUrl(post.Id),
                ForumId = post.Forum.Id
            });

            return new HomeIndexModel()
            {
                LatestPosts = posts
            };

        }

        // Search
        [HttpPost]
        public IActionResult Search(string searchQuery)                          // Search пошук постів
        {
            return RedirectToAction("Topic", "Forum", new { searchQuery });
        }

    }
}
