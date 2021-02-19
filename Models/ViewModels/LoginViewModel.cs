using System.ComponentModel.DataAnnotations;

namespace AmiSocialWebApi.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; }
    }
}