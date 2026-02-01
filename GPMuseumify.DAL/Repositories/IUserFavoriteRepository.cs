

using GPMuseumify.DAL.Models;

namespace GPMuseumify.DAL.Repositories;

public interface IUserFavoriteRepository
{
    Task<IReadOnlyList<UserFavorite>> GetUserFavoritesAsync(Guid userId, int skip, int take);
    Task<int> CountByUserIdAsync(Guid userId);

    Task<UserFavorite?> AddAsync(UserFavorite favorite);
    Task<UserFavorite?> GetByUserAndItemAsync(Guid userId, Guid? statueId, Guid? museumId);
    Task<UserFavorite?> GetByIdAsync(Guid favoriteId);
    Task<bool> RemoveAsync(Guid favoriteId);
    Task<bool> ExistsAsync(Guid userId, Guid? statueId, Guid? museumId);

}
