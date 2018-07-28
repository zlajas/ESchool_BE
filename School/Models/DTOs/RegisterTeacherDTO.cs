using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace School.Models.DTOs
{
    public class RegisterTeacherDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Username must be between 3 and 30 character in length.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 3 and 30 character in length.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be between 3 and 30 character in length.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}