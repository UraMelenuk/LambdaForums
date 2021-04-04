using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LambdaForums.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser           // Користувач (розширюєм нашого користувача)
    {
        public string UserDescription { get; set; }
        public int Rating { get; set; }                   // Рейтинг Користувача
        public string ProfileImageUrl { get; set; }       // Зображення профілю користувача
        public DateTime MemberSince { get; set; }         // Дата запамятати коли були створені
        public bool IsActive { get; set; }                // Активний , чи не активний Користувач
        public bool IsAdmin { get; set; }                 // Чи є адміном

    }
}
