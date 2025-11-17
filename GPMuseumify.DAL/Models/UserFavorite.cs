using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMuseumify.DAL.Models;

public class UserFavorite
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public Guid? StatueId { get; set; }

    public Guid? MuseumId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("StatueId")]
    public virtual Statue? Statue { get; set; }

    [ForeignKey("MuseumId")]
    public virtual Museum? Museum { get; set; }
}