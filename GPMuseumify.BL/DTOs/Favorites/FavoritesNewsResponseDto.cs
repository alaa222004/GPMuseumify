

using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.Favorites;

public class FavoritesNewsResponseDto
{
    public Guid UserId { get; set; }
    public List<FavoriteNewsItemDto> Items { get; set; } = new();
    public PaginationDto Pagination { get; set; }= new();


}
