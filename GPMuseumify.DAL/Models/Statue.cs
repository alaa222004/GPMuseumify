using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.DAL.Models;

public class Statue
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAr { get; set; }

    public string? Description { get; set; }

    public string? DescriptionAr { get; set; }

    [MaxLength(100)]
    public string? HistoricalPeriod { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(200)]
    public string? Museum { get; set; }

    [Required]
    [MaxLength(500)]
    public string VideoUrl { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<StatueImage> StatueImages { get; set; } = new List<StatueImage>();
    public virtual ICollection<UserHistory> UserHistories { get; set; } = new List<UserHistory>();
    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
}
