

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMuseumify.DAL.Models;

public class ImageRecognitionLog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; }
    
    [MaxLength(500)]
    public string? UploadedImageUrl { get; set; }

    public Guid? RecognizedStatueId { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Confidence { get; set; } // Confidence percentage

    [MaxLength(50)]
    public string? RecognitionMethod { get; set; } // 'scan', 'upload', 'search'

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    [ForeignKey("RecognizedStatueId")]
    public virtual Statue? RecognizedStatue { get; set; }
}

