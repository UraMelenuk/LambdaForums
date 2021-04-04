using Microsoft.AspNetCore.Http;
using System;

namespace LambdaForums.Models.ApplicationUserL
{
    public class ProfileModel                         // клас ProfileModel
    {
        public string UserId { get; set; }            // Id користувача
        public string Email { get; set; }             // Адреса почти користувача 
        public string UserName { get; set; }          // Ім'я користувача
        public string UserRating { get; set; }        // Кейтинг користувача
        public string ProfileImageUrl { get; set; }   // Шлях картинки користувача
        public DateTime MemberSince { get; set; }     // Дата 
        public bool IsAdmin { get; set; }             // Чи є адміном

        public IFormFile ImageUpload { get; set; }    // Завантаженна картинка
    }
}
