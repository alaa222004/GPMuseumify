namespace GPMuseumify.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
