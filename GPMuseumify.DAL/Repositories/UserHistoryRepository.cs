

using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMuseumify.DAL.Repositories;

public class UserHistoryRepository : IUserHistoryRepository 
{
    private readonly ApplicationDbContext _context;
    public UserHistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UserHistory> AddAsync(UserHistory history)
    {
    _context.UserHistories.Add(history);
        await _context.SaveChangesAsync();
        return history;

    }

    public async Task<int> CountByUserIdAsync(Guid userId)
    {
       return await _context.UserHistories.AsNoTracking()
            .CountAsync(history => history.UserId == userId);
    }

    public async Task<UserHistory?> GetByIdWithDetailsAsync(Guid id)// يجيب عنصر من التاريخ بتاع اليوزر مع التفاصيل بتاعته
    {
       return await _context.UserHistories
            .AsNoTracking()
            .Include(history=> history.Statue).
            Include(history=> history.Museum).
            FirstOrDefaultAsync(history => history.Id == id);
    }

    public async Task<IReadOnlyList<UserHistory>> GetUserHistoryAsync(Guid userId, int skip, int take)
    {
       return await _context.UserHistories.AsNoTracking()
            .Where(history => history.UserId == userId).
            Include(history => history.Statue)
            .Include(history => history.Museum)
            .OrderByDescending(history => history.ViewedAt).
            Skip(skip).
            Take(take)
            .ToListAsync();
    }
}
