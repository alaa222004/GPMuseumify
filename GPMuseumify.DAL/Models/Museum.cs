using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.DAL.Models;

public class Museum
{
    
        [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAr { get; set; }

    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<UserHistory> UserHistories { get; set; } = new List<UserHistory>();
    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
}
