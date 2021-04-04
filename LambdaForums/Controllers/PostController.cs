using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForums.Models;
using LambdaForums.Models.PostL;
using LambdaForums.Models.ReplyL;
using LambdaForums.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    public class PostController : Controller                        // Post Controller
    {
        private readonly IPost _postService;                        // Екземпляр інтерфейса IPost
        private readonly IForum _forumService;                      // Екземпляр інтерфейса IForum
        private readonly IPostReply _replyService;                  // Екземпляр інтерфейса IPostReply
        private readonly IApplicationUser _userService;             // Екземпляр інтерфейса IApplicationUser
        private static UserManager<ApplicationUser> _userManager;   // Екземпляр UserManager користувачів



        // Конструктор Контролера з параметрами IPost , IPostReply , IForum , UserManager , IApplicationUser
        public PostController(IPost postService , 
                              IPostReply replyService, 
                              IForum forumService , 
                              UserManager<ApplicationUser> userManager ,
                              IApplicationUser userService)   
        {
            _postService = postService;
            _forumService = forumService;
            _replyService = replyService;
            _userService = userService;
            _userManager = userManager;
        }

        // Index
        public IActionResult Index(int id)                         // Повернути все пости по Id 
        {
            var post = _postService.GetById(id);

            var replies = GetPostReplies(post).OrderBy(reply => reply.Created);   // метод GetPostReplies    BuildPostReplies 

            var model = new PostIndexModel                         // Модель (повернемо на нову сторінку PostIndexModel)
            {
                Id = post.Id,                                      // Id
                Title = post.Title,                                // Заголовок
                AuthorId = post.User.Id,                           // Користувач id
                AuthorName = post.User.UserName,                   // Ім'я користувача
                AuthorImageUrl = post.User.ProfileImageUrl,        // Зображення користувача
                AuthorRating = post.User.Rating,                   // Рейтинг користувача
                IsAuthorAdmin = IsAuthorAdmin(post.User),          // чи є адміном
                Date = post.Created,                               // Дата створення
                PostContent = post.Content,                        // Зміст 
                Replies = replies,                                 // Відповіді
                ForumId = post.Forum.Id,                           // Форум id
                ForumName = post.Forum.Title,                      // Форум назва
            };

            return View(model);
        }

        // GetPostReplies
        private IEnumerable<PostReplyModel> GetPostReplies(Post post)   // Метод GetPostReplies повернути всі відповіді в пості
        {
            return post.Replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }

        // Create
        [Authorize]
        public IActionResult Create(int id)                       // Create пост
        {
            // id is Forum.Id
            var forum = _forumService.GetById(id);

            var model = new NewPostModel                          // модель (повернемо на нашу нову сторінку NewPostModel)
            {
                ForumId = forum.Id,                               // id форуму
                ForumName = forum.Title,                          // назва форуму
                ForumImageUrl = forum.ImageUrl,                   // зображення
                AuthorName = User.Identity.Name,                  // користувач який зареєструвався на форумі
            };

            return View(model);                                   // повернем на сторінку яку створили
             
        }

        // Edit
        [Authorize]
        public IActionResult Edit(int id)                         // Edit пост
        {
            var post = _postService.GetById(id);

            var model = new EditPostModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Created = DateTime.Now,
                Forum = post.Forum
            };

            return View(model);
        }

        // ConfirmEdit
        [HttpPost]
        [ActionName("Edit")]
        [Authorize]
        public async Task<IActionResult> ConfirmEdit(int id, string title, string content)   // ConfirmEdit пост
        {
            if (ModelState.IsValid)
            {
                var post = _postService.GetById(id);

                await _postService.EditPostContent(id, title, content);

                return RedirectToAction("Index", "Post", new { id = post.Id });
            }
            return View();
        }

        // Delete
        [Authorize]
        public IActionResult Delete(int id)                       // Delete Post
        {
            var post = _postService.GetById(id);

            var model = new DeletePostModel
            {
                PostId = post.Id,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created,
                User = post.User,
                Replies = post.Replies

            };

            return View(model);
        }

        // ConfirmDelete
        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)                // ConfirmDelete Post
        {
            var post = _postService.GetById(id);
            var replies = GetPostReplies(post);

            if (replies.Count() > 0)
            {
                for (int i = 1; i <= replies.Count();)
                {
                    await _replyService.Delete(replies.FirstOrDefault().Id);
                }
            }

            await _postService.Delete(id);

            return RedirectToAction("Topic", "Forum", new { id = post.Forum.Id });
        }

        // AddPost
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)   // Асинхронний потік добавлення нової публікації зі сторінки
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                var post = BuildPost(model, user);

                await _postService.Add(post);
                await _userService.BumpRating(userId, typeof(Post));

                return RedirectToAction("Index", "Forum", model.ForumId);
            }

            return View();
        }

        // IsAuthorAdmin
        public static bool IsAuthorAdmin(ApplicationUser user)             // Асинхронний метод чи є користувач адміном
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }

        // BuildPsot
        private Post BuildPost(NewPostModel model, ApplicationUser user)   // Метод побудувати пост (з декількох моделей)
        {
            var forum = _forumService.GetById(model.ForumId);              // id forum

            return new Post                    // Новий пост
            {
                Title = model.Title,           // Заголовок
                Content = model.Content,       // Зміст
                Created = DateTime.Now,        // Дата створення now 
                User = user,                   // Користувач
                Forum = forum                  // Форум id
            };
        }

    }
}
