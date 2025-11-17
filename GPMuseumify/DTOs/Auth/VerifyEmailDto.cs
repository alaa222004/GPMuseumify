using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.DTOs.Auth;

public class VerifyEmailDto
{
    [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Verification code is required")]
    [StringLength(6, MinimumLength = 4, ErrorMessage = "Verification code must be 4-6 characters")]
    public string Code { get; set; } = string.Empty;
}
