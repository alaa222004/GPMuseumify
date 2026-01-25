

namespace GPMuseumify.BL.DTOs.News;

public class EventDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }
    public DateTime EventDate { get; set; }
    public string? Location { get; set; }
    public string? LocationAr { get; set; }
    public DateTime PublishedAt { get; set; }
    public string? SourceName { get; set; }
}
