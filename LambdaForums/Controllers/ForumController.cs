using LambdaForums.Models;
using LambdaForums.Models.ForumL;
using LambdaForums.Models.PostL;
using LambdaForums.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LambdaForums.Controllers
{
    public class ForumController : Controller             // ForumController
    {
        private readonly IForum _forumService;            // Екземпляр нашого інтерфейса IForum
        private readonly IPost _postService;              // Екземпляр нашого інтерфейса IPost
        private readonly IPostReply _replyService;        // Екземпляр нашого інтерфейса IPostReply
        private readonly IUpload _uploadService;          // Екземпляр нашого інтерфейса IUpload
        private readonly IConfiguration _configuration;   // Екземпляр нашого інтерфейса IConfiguration (Azure)

                                                          //  Конструктор з параметрами
        public ForumController(IForum forumService ,
                               IPost postService,
                               IPostReply replyService,
                               IUpload uploadService ,
                               IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _replyService = replyService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        // Index
        public IActionResult Index()                                // повернем всі форуми  Index
        {
            var forums = _forumService.GetAll().Select(f => new ForumListingModel
            {
                    Id = f.Id,
                    Name = f.Title,
                    Description = f.Description,
                    NumberOfPosts = f.Posts?.Count() ?? 0,
                    Latest = GetLatestPost(f.Id) ?? new PostListingModel(),
                    NumberOfUsers = _forumService.GetActiveUsers(f.Id).Count(),
                    ImageUrl = f.ImageUrl,
                    HasRecentPosts = _forumService.HasRecentPost(f.Id)

            });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Name)
            };

            return View(model);
        }

        // GetLatestPost
        private PostListingModel GetLatestPost(int forumId)         // GetLatestPost повернути останні пости
        {
            var post = _forumService.GetLatestPost(forumId);

            if (post != null)
            {
                return new PostListingModel
                {
                    AuthorName = post.User != null ? post.User.UserName : "",
                    DatePosted = post.Created.ToString(CultureInfo.InvariantCulture),
                    Title = post.Title ?? ""
                };
            }

            return new PostListingModel();
        }

        // Topic
        public IActionResult Topic(int id , string searchQuery)     // Topic 
        {
            var forum = _forumService.GetById(id);
            var posts = _forumService.GetFilteredPosts(id, searchQuery).ToList();
            var noResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Forum = BuildForumListing(post),
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(CultureInfo.InvariantCulture),
                RepliesCount = post.Replies.Count()
            }).OrderByDescending(post => post.DatePosted);

            var model = new TopicResultModel
            {
                EmptySearchResults = noResults,
                Posts = postListings,
                SearchQuery = searchQuery,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        // Search
        [HttpPost]
        public IActionResult Search(int id , string searchQuery)    // Search  пошук постів на форумі
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        //Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()                               // Create створити нову модель форуму 
        {
            var model = new AddForumModel();
            return View(model);
        }

        // AddForum
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddForum(AddForumModel model)   // AddForum асинхронний метод додавання форуму
        {
            var imageUri = "";

            if (model.ImageUpload != null)                               // якщо зображення не є пустим
            {
                var blockBlob = UploadForumImage(model.ImageUpload);     // завантажуєм з Azure сховища
                imageUri = blockBlob.Uri.AbsoluteUri;
            }
            else
            {
                imageUri = "/images/users/default.png";                  // зображення по замовчуванням
            }

            var forum = new Forum                                        // створити новий форум
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Create(forum);

            return RedirectToAction("Index", "Forum");
            
        }

        // Delete
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)                         // Delete форум 
        {
            var forum = _forumService.GetById(id);
            var post = _forumService.GetLatestPost(forum.Id);

            var model = new DeleteForumModel
            {
                ForumId = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                Created = forum.Created,
                ImageUrl = forum.ImageUrl,
            };

            return View(model);
        }

        // ConfirmDelete
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)                 // ConfirmDelete форум
        {
            var forum = _forumService.GetById(id);

            if (forum.Posts.Count() > 0)
            {
                for (int i = 1; i <= forum.Posts.Count();)
                {
                    var reply = _forumService.GetLatestPost(forum.Id).Replies;

                    if (reply.Count() > 0)
                    {
                        for (int j = 1; j <= reply.Count();)
                        {
                            await _replyService.Delete(reply.LastOrDefault().Id);
                        }
                    }
                    reply = null;

                    await _postService.Delete(forum.Posts.LastOrDefault().Id);
                }
            }

            await _forumService.Delete(id);

            return RedirectToAction("Index", "Home");
        }

        // Edit
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)                                      // Edit форум
        {
            var forum = _forumService.GetById(id);

            var model = new NewForumModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl,
                Created = DateTime.Now
            };

            return View(model);
        }

        // ConfirmEdit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(NewForumModel model)        // ConfirmEdit
        {
            var imageUri = "";

            if (model.ImageUpload != null)                               // якщо зображення не є пустим
            {
                var blockBlob = UploadForumImage(model.ImageUpload);     // завантажуєм з Azure сховища
                imageUri = blockBlob.Uri.AbsoluteUri;

                model.ImageUrl = imageUri;
            }
            else
            {
                imageUri = "/images/users/default.png";                  // зображення по замовчуванням
            }

            await _forumService.Update(model.Id,model.Title,model.Description, model.ImageUrl);

            return RedirectToAction("Index", "Forum");
        }

        // UoloadForumImage
        private CloudBlockBlob UploadForumImage(IFormFile file)          // Метод загрузки зображення з хмари Azure
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");       // connection Azure
            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");      // container (шлях , папка)
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = contentDisposition.FileName.Trim('"');                      // імя файлу
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStreamAsync(file.OpenReadStream()).Wait();             // асинхронний потік відкритого читтаня який є у файлі (щоб завантажити у хмару)

            return blockBlob;
        }

        // BuildForumListing Post
        private ForumListingModel BuildForumListing(Post post)       // Метод BuildForumListing (Post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        // BuildForumListing Forum
        private ForumListingModel BuildForumListing(Forum forum)     // Метод BuildForumListing (Forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}