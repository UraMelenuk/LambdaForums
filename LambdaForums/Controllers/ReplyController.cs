using System;
using System.Threading.Tasks;
using LambdaForums.Models;
using LambdaForums.Models.ReplyL;
using LambdaForums.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    [Authorize]
    public class ReplyController : Controller                 // ReplyController
    {
        private readonly IForum _forumService;                        // Екземпляр інтерфейса IForum
        private readonly IPost _postService;                          // Екземпляр інтерфейса IPost
        private readonly IPostReply _postReplyService;                // Екземпляр інтерфейса IPostReply
        private readonly IApplicationUser _userService;               // Екземпляр інтерфейса IApplicationUser
        private readonly UserManager<ApplicationUser> _userManager;   // Екземпляр UserManager користувачів
        

                                                                      // Конструктор з параметрами 
        public ReplyController(IForum forumService, 
                               IPost postService,
                               IPostReply postReplyService,
                               IApplicationUser userService,
                               UserManager<ApplicationUser> userManager)   
        {
            _forumService = forumService;
            _postService = postService;
            _postReplyService = postReplyService;
            _userService = userService;
            _userManager = userManager;
        }

        // Create
        public async Task<IActionResult> Create(int id)                   // Create асинхронний потік створити
        {
            if (ModelState.IsValid)
            {
                var post = _postService.GetById(id);
                var forum = _forumService.GetById(post.Forum.Id);
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var model = new PostReplyModel
                {
                    PostId = post.Id,
                    PostTitle = post.Title,
                    PostContent = post.Content,

                    ForumId = post.Forum.Id,
                    ForumName = post.Forum.Title,
                    ForumImageUrl = post.Forum.ImageUrl,

                    AuthorId = user.Id,
                    AuthorName = User.Identity.Name,
                    AuthorImageUrl = user.ProfileImageUrl,
                    AuthorRating = user.Rating,
                    IsAuthorAdmin = user.IsAdmin,
                    Created = DateTime.Now
                };

                return View(model);
            }
            return View();
        }

        // Edit
        public IActionResult Edit(int id)                                 // Edit reply редагувати відповідь по id
        {
            var reply = _postReplyService.GetById(id);

            var model = new EditPostReplyModel
            {
                Id = reply.Id,
                Message = reply.Content,
                Created = DateTime.Now,
                Post = reply.Post
            };

            return View(model);
        }

        // ConfirmEdit
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(int id, string message)   // Confirm Edit підтвердження редагування відповіді
        {
            if (ModelState.IsValid)
            {
                var reply = _postReplyService.GetById(id);

                await _postReplyService.Update(id, message);

                return RedirectToAction("Index", "Post", new { id = reply.Post.Id });
            }
            return View();
        }

        // Delete
        public IActionResult Delete(int id)                               // Delete reply
        {
            var reply = _postReplyService.GetById(id);

            var model = new DeletePostReplyModel
            {
                Id = reply.Id,
                Content = reply.Content,
                Created = reply.Created,
                Post = reply.Post,
                User = reply.User
            };

            return View(model);
        }

        // ConfirmDelete
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)                // ConfirmDelete reply
        {
            var reply = _postReplyService.GetById(id);

            _postReplyService.Delete(id);

            return RedirectToAction("Index", "Post", new { id = reply.Post.Id });
        }

        // AddReply
        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)   // AddReply додати відповідь
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildReply(model, user);
            await _postService.AddReply(reply);
            await _userService.BumpRating(userId, typeof(PostReply));

            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        // BuildReply
        private PostReply BuildReply(PostReplyModel model, ApplicationUser user)   // Метод BuildReply передаєм модель відповіді і користувача
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}