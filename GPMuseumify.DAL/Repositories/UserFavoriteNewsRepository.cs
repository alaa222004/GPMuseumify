
using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMuseumify.DAL.Repositories;

public class UserFavoriteNewsRepository : IUserFavoriteNewsRepository
{
    private readonly ApplicationDbContext _context;
    public UserFavoriteNewsRepository (ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserFavoriteNews?> AddAsync(UserFavoriteNews favorite)
    {

        if (await ExistsAsync(favorite.UserId, favorite.ItemId, favorite.ItemType))
            return null;

        _context.UserFavoriteNews.Add(favorite);
        await _context.SaveChangesAsync();
        return favorite;
    }

    public async Task<int> CountByUserIdAsync(Guid userId)
    {
       return await _context.UserFavoriteNews
            .AsNoTracking()
            .CountAsync(f=>f.UserId == userId);
    }

    public async Task<bool> ExistsAsync(Guid userId, string itemId, string itemType)
    {
       return await _context.UserFavoriteNews
            .AsNoTracking()
            .AnyAsync(f=>f.UserId == userId && f.ItemId == itemId && f.ItemType == itemType);
    }

    public async Task<IReadOnlyList<UserFavoriteNews>> GetUserFavoriteNewsAsync(Guid userId, int skip, int take)
    {
     return await _context.UserFavoriteNews
            .AsNoTracking()
            .Where(f=>f.UserId == userId)
            .OrderByDescending(f=>f.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<bool> RemoveAsync(Guid userId, string itemId, string itemType)
    {
    var fav=await _context.UserFavoriteNews
            .FirstOrDefaultAsync(f=>f.UserId == userId && f.ItemId == itemId && f.ItemType == itemType);
        if (fav == null)
            return false;
        _context.UserFavoriteNews.Remove(fav);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveByIdAsync(Guid userId, Guid favoriteId)
    {
        var fav = await _context.UserFavoriteNews
            .FirstOrDefaultAsync(f => f.Id == favoriteId && f.UserId == userId);
        if (fav == null)
            return false;
        _context.UserFavoriteNews.Remove(fav);
        await _context.SaveChangesAsync();
        return true;
    }
}
