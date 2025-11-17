using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Auth;

public class SocialLoginDto
{
    [Required(ErrorMessage = "Provider is required")]
    [RegularExpression("google|apple", ErrorMessage = "Provider must be 'google' or 'apple'")]
    public string Provider { get; set; } = string.Empty; //  Google, apple
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;
}