using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Auth;

public class ResetPasswordDto
{

    //[Required(ErrorMessage = "Email is required")]
    //[EmailAddress]
    //public string Email { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Verification code is required")]
    //[StringLength(6, MinimumLength = 4)]
    //public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "Password and confirmation do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
