

namespace GPMuseumify.BL.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    //public string Token { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public bool IsEmailVerified { get; set; }
    public string? PhoneNumber { get; set; }
}
