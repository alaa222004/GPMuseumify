
using GPMuseumify.BL.DTOs.Favorites;

namespace GPMuseumify.BL.Interfaces;

public interface IFavoritesNewsService
{
    Task<FavoritesNewsResponseDto> GetUserFavoriteNewsAsync(Guid userId, int page, int pageSize);
    Task<FavoriteNewsItemDto?> AddFavoriteNewsAsync(Guid userId, AddFavoriteNewsDto dto);
    Task<bool> RemoveFavoriteNewsAsync(Guid userId, string itemId ,string itemType);
    Task<bool> IsFavoriteNewsAsync(Guid userId, string itemId, string itemType);
}
