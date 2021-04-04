using LambdaForums.Models;
using LambdaForums.Models.ApplicationUserL;
using LambdaForums.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LambdaForums.Controllers
{
    [Authorize]
    public class ProfileController : Controller                       // Profile Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;   // Екземпляр нашого обекта User
        private readonly IApplicationUser _userService;               // Екземпляр нашого інтерфейса IUser
        private readonly IUpload _uploadService;                      // Екземпляр нашого інтерфейса IUpload (Загрузка зображення)
        private readonly IConfiguration _configuration;               // Екземпляр нашого Microsoft Azure

                                                                            // Конструктор з параметрами
        public ProfileController(UserManager<ApplicationUser> userManager, 
                                            IApplicationUser userService , 
                                            IUpload uploadService, 
                                            IConfiguration configuration)   
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()                                   // Index
        {
            var profiles = _userService.GetAll()                       // повернути всіх користувачів
                .OrderByDescending(user => user.Rating)                // порядок по спаданню рейтингу
                .Select(u => new ProfileModel
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince
                });

            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }

        public IActionResult Detail(string id)                         // Detail user
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)  // Завантаження зображення в профіль користувачу
        {
            var userId = _userManager.GetUserId(User);                       // повернути id user

            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");       // connection Azure

            var container = _uploadService.GetBlobContainer(connectionString, "profile-images");    // container Azure (шлях , папка)

            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var filename = contentDisposition.FileName.Trim('"');                      // імя файлу

            var blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());              // асинхронний потік відкритого читтаня який є у файлі (щоб завантажити у хмару)

            await _userService.SetProfileImage(userId, blockBlob.Uri);                 // і завантажити користувачу

            return RedirectToAction("Detail", "Profile", new { id = userId });         // і повернути frofile користувача

        }

    }
}