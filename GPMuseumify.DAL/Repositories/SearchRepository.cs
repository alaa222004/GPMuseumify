
using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMuseumify.DAL.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly ApplicationDbContext _context;
    public SearchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Statue>> SearchStatuesAsync(string query, int skip, int take)
    {
        var searchTerm = query.Trim().ToLower();
        return await _context.Statues.AsNoTracking().
            Where(s => s.IsActive && (s.Name.ToLower().Contains(searchTerm) ||  // search in name
                     (s.NameAr != null && s.NameAr.ToLower().Contains(searchTerm)) ||  // search in Arabic name
                     (s.Description != null && s.Description.ToLower().Contains(searchTerm)) ||  // search in description
                     (s.DescriptionAr != null && s.DescriptionAr.ToLower().Contains(searchTerm)) ||  // search in Arabic description
                     (s.HistoricalPeriod != null && s.HistoricalPeriod.ToLower().Contains(searchTerm)) ||  
                     (s.Location != null && s.Location.ToLower().Contains(searchTerm)) ||  // search in location
                     (s.Museum != null && s.Museum.ToLower().Contains(searchTerm)))).OrderByDescending(s => s.Name.ToLower().StartsWith(searchTerm) ? 0 : 1)  // اللي اسمه بيبدأ بالكلمة → يطلع الأول
                .ThenBy(s => s.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
    }
    public async Task<IReadOnlyList<Museum>> SearchMuseumsAsync(string query, int skip, int take)
    {
        var searchTerm = query.Trim().ToLower(); //msh btkhleha sensitive l case
        return await _context.Museums.AsNoTracking().
            Where(m => m.IsActive && (m.Name.ToLower().Contains(searchTerm) ||  // search in name
                     (m.NameAr != null && m.NameAr.ToLower().Contains(searchTerm)) ||  // search in Arabic name
                     (m.Description != null && m.Description.ToLower().Contains(searchTerm)) ||  // search in description
                     (m.Location != null && m.Location.ToLower().Contains(searchTerm)))).OrderByDescending(m => m.Name.ToLower().StartsWith(searchTerm) ? 0 : 1)  // اللي اسمه بيبدأ بالكلمة → يطلع الأول
                .ThenBy(m => m.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
    }

    public async Task<int> CountStatuesAsync(string query)
    {
        var searchTerm = query.Trim().ToLower();
        return await _context.Statues.AsNoTracking()
             .CountAsync(s => s.IsActive && (s.Name.ToLower().Contains(searchTerm) ||  //bt3d el sfof
                     (s.NameAr != null && s.NameAr.ToLower().Contains(searchTerm)) ||  
                     (s.Description != null && s.Description.ToLower().Contains(searchTerm)) || 
                     (s.DescriptionAr != null && s.DescriptionAr.ToLower().Contains(searchTerm)) ||  
                     (s.HistoricalPeriod != null && s.HistoricalPeriod.ToLower().Contains(searchTerm)) || 
                     (s.Location != null && s.Location.ToLower().Contains(searchTerm)) ||  
                     (s.Museum != null && s.Museum.ToLower().Contains(searchTerm))));
           
    }

    public async Task<int> CountMuseumsAsync(string query)
    {
        var searchTerm = query.Trim().ToLower();
        return await _context.Museums.AsNoTracking().
         CountAsync(m => m.IsActive && (m.Name.ToLower().Contains(searchTerm) ||
                        (m.NameAr != null && m.NameAr.ToLower().Contains(searchTerm)) ||  
                        (m.Description != null && m.Description.ToLower().Contains(searchTerm)) ||  
                        (m.Location != null && m.Location.ToLower().Contains(searchTerm))));



    }


    public async Task<IReadOnlyList<Statue>> GetPopularStatuesAsync(int count)
    {
        return await _context.Statues.AsNoTracking()
            .Where(s => s.IsActive).OrderByDescending(s => s.UserHistories.Count(uh => uh.StatueId == s.Id)). // popular based on number of user histories
            ThenByDescending(s => s.CreatedAt).Take(count).ToListAsync();
         }

    public async Task<IReadOnlyList<Museum>> GetPopularMuseumsAsync(int count)
    {
        return await _context.Museums
                  .AsNoTracking()
                  .Where(m => m.IsActive)
                  .OrderByDescending(m => _context.UserHistories.Count(uh => uh.MuseumId == m.Id))
                  .ThenByDescending(m => m.CreatedAt)
                  .Take(count)
                  .ToListAsync();
    }

}
