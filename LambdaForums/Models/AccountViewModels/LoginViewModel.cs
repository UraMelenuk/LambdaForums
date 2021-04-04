using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.AccountViewModels
{
    public class LoginViewModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }            // UserEmail

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }         // Password

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }         // Remember Me
    }
}
