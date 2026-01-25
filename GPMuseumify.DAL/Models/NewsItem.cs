

namespace GPMuseumify.DAL.Models;

public class NewsItem
{
    public string? Id { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime PublishedAt { get; set; }
    public string? SourceName { get; set; }
    public string? Category { get; set; }
}
