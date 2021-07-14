using System;
using System.ComponentModel.DataAnnotations;

namespace AmiSocialWebApi.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FamilyNickname { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}