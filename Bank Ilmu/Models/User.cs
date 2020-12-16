using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bank_Ilmu.Models
{
    public class User
    {
        [Required(ErrorMessage ="error")]
        public string username { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [MinLength(8, ErrorMessage = "Minimal 8 karakter")]
        public string password { get; set; }
        [MinLength(8, ErrorMessage = "Minimal 8 karakter")]
        public string rePassword { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "error")]
        public string username { get; set; }
        [MinLength(8, ErrorMessage = "Minimal 8 karakter")]
        public string password { get; set; }
    }
}