using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Auth;

public class ResetPasswordDto
{
    [Required (ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;//eli atb3tlo 3la el email
    [Required(ErrorMessage = "New password is required")]
    [MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;
}
