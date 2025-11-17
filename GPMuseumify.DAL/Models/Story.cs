using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMuseumify.DAL.Models;

public class Story
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? TitleAr { get; set; }

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public Guid? StatueId { get; set; } // Optional: may be linked to a statue

    [MaxLength(100)]
    public string? Category { get; set; } // 'general', 'history', 'museum', etc.

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("StatueId")]
    public virtual Statue? Statue { get; set; }
}