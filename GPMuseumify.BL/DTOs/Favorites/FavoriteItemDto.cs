

namespace GPMuseumify.BL.DTOs.Favorites;

public class FavoriteItemDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? StatueId { get; set; }
    public Guid? MuseumId { get; set; }
    public string ContentType { get; set; } = "statue";
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }


}
