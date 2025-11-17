

using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Auth;

public class RegisterDto
{
    [Required (ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string Password { get; set; } = string.Empty;

     [RegularExpression("User|Admin", ErrorMessage = "Role must be either 'User' or 'Admin'")]
    public string Role { get; set; }="User";
}
