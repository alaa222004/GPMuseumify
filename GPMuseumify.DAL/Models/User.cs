
using System.ComponentModel.DataAnnotations;


namespace GPMuseumify.DAL.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }= Guid.NewGuid();
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }
    [MaxLength(255)]
    public string? GoogleId { get; set; }

    [MaxLength(255)]
    public string? AppleId { get; set; }


    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "User"; // "User" or "Admin"

    public bool IsEmailVerified { get; set; } = false;

    [MaxLength(255)]
    public string? EmailVerificationToken { get; set; }
    [MaxLength(10)]
    public string? EmailVerificationCode { get; set; }
    public DateTime? EmailVerificationExpiry { get; set; }

    [MaxLength(255)]
    public string? ResetPasswordToken { get; set; }

    public DateTime? ResetPasswordExpiry { get; set; }

    public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

    [MaxLength(5)]
    public string PreferredLanguage { get; set; } = "en";
    public bool NotificationsEnabled { get; set; } = true; 

    public virtual ICollection<UserHistory> UserHistories { get; set; } = new List<UserHistory>();
    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    public virtual ICollection<UserFavoriteNews> UserFavoriteNews { get; set; } = new List<UserFavoriteNews>();
}


