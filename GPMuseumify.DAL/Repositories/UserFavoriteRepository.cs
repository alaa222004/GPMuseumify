

using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMuseumify.DAL.Repositories;

public class UserFavoriteRepository : IUserFavoriteRepository
{
    private readonly ApplicationDbContext _context;
    public UserFavoriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UserFavorite?> AddAsync(UserFavorite favorite)
    {
        if(await ExistsAsync(favorite.UserId, favorite.StatueId, favorite.MuseumId))
        {
            return null;
        }
        _context.UserFavorites.Add(favorite);
        await  _context.SaveChangesAsync();
        return favorite;
    }

    public Task<int> CountByUserIdAsync(Guid userId)
    {
        return _context.UserFavorites
            .AsNoTracking().
            CountAsync(f => f.UserId == userId);
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid? statueId, Guid? museumId)
    {
        if (statueId.HasValue)
        {
            return await _context.UserFavorites
                .AnyAsync(f => f.UserId == userId && f.StatueId == statueId);

        }
        if (museumId.HasValue)
        {
            return await _context.UserFavorites
                .AnyAsync(f => f.UserId == userId && f.MuseumId == museumId);
        }
        return false;
    }

    public async Task<UserFavorite?> GetByIdAsync(Guid favoriteId)
    {
        return await _context.UserFavorites
            .AsNoTracking()
            .Include(f=>f.Statue)
            .Include(f=>f.Museum)
            .FirstOrDefaultAsync(f=>f.Id==favoriteId);
    }

    public async Task<UserFavorite?> GetByUserAndItemAsync(Guid userId, Guid? statueId, Guid? museumId)
    {
      return await _context.UserFavorites
            .AsNoTracking()
            .Include(f=>f.Statue)
            .Include(f=>f.Museum)
            .FirstOrDefaultAsync(f=>f.UserId==userId &&
            ((statueId.HasValue && f.StatueId == statueId)
            || (museumId.HasValue && f.MuseumId == museumId)));
    }

    public async Task<IReadOnlyList<UserFavorite>> GetUserFavoritesAsync(Guid userId, int skip, int take)
    {
        return await _context.UserFavorites
                 .AsNoTracking()
                 .Where(f => f.UserId == userId)
                 .Include(f => f.Statue)
                 .Include(f => f.Museum)
                 .OrderByDescending(f => f.CreatedAt)
                 .Skip(skip)
                 .Take(take)
                 .ToListAsync();
    }

    public async Task<bool> RemoveAsync(Guid favoriteId)
    {
        var fav = await _context.UserFavorites.FindAsync(favoriteId);
        if (fav == null)
        {
            return false;
        }
        _context.UserFavorites.Remove(fav);
        await _context.SaveChangesAsync();
        return true;
    }
}
