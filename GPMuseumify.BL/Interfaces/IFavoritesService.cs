

using GPMuseumify.BL.DTOs.Favorites;

namespace GPMuseumify.BL.Interfaces;

public interface IFavoritesService
{
    Task<FavoritesResponseDto> GetUserFavoritesAsync(Guid userId, int page, int pageSize);
    Task<FavoriteItemDto?> AddFavoriteAsync(Guid userId, AddFavoriteDto dto);
    Task<bool> RemoveFavoriteAsync(Guid userId, Guid favoriteId);
    Task<bool> RemoveFavoriteByItemAsync(Guid userId, Guid? statueId, Guid? museumId);
    Task<bool> IsFavoriteAsync(Guid userId, Guid? statueId, Guid? museumId);
}
