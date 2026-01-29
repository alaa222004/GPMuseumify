
using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.History;

public class CreateHistoryEntryDto
{
    public Guid? StatueId { get; set; }
    public Guid? MuseumId { get; set; }
    [MaxLength(50)]
    public string? SearchType { get; set; }

    public DateTime? ViewedAt { get; set; }

}
