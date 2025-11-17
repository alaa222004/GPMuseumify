using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMuseumify.DAL.Models;

public class StatueImage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StatueId { get; set; }

    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(64)]
    public string? ImageHash { get; set; }

    public bool IsPrimary { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey("StatueId")]
    public virtual Statue Statue { get; set; } = null!;
}