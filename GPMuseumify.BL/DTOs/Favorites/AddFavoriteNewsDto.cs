

using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Favorites;

public class AddFavoriteNewsDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    [RegularExpression("^(news|event)$", ErrorMessage = "ItemType must be either 'news' or 'event'.")]
    public string ItemType { get; set; } = "news";
}
