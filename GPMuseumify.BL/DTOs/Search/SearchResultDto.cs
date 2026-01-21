
namespace GPMuseumify.BL.DTOs.Search;

public class SearchResultDto
{
    public Guid  Id { get; set; }
    public string Name { get; set; } = string.Empty;

   public string? NameAr { get; set; } 
    public string? Description { get; set; } 
    public string? DescriptionAr { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }

    public string Type { get; set; } = string.Empty; // "statue" or "museum"
    public string? HistoricalPeriod { get; set; } // For statues
    public string? Museum { get; set; } // For statues

}
