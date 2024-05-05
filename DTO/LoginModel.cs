using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
    public class UpdateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        
    }

}
