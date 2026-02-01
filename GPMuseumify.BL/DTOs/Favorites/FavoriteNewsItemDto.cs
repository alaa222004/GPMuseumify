

namespace GPMuseumify.BL.DTOs.Favorites;

public class FavoriteNewsItemDto
{
    public Guid FavoriteId { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemType { get; set; } = "news";
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public string? LocationAr { get; set; }
    public string? SourceName { get; set; }
    public DateTime CreatedAt { get; set; }



}
