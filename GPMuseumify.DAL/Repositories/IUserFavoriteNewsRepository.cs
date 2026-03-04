

using GPMuseumify.DAL.Models;

namespace GPMuseumify.DAL.Repositories;

public interface IUserFavoriteNewsRepository
{
    Task<IReadOnlyList<UserFavoriteNews>> GetUserFavoriteNewsAsync(Guid userId, int skip, int take);
    Task<int> CountByUserIdAsync(Guid userId);
    Task<UserFavoriteNews?> AddAsync(UserFavoriteNews favorite);
    Task<bool> RemoveAsync(Guid userId, string itemId, string itemType);
    Task<bool> RemoveByIdAsync(Guid userId, Guid favoriteId);
    Task<bool> ExistsAsync(Guid userId, string itemId, string itemType);
}
