

using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.Favorites;

public class FavoritesResponseDto
{
    public Guid UserId { get; set; }
    public List<FavoriteItemDto> Items { get; set; } = new();
    public PaginationDto Pagination { get; set; }= new();
}
