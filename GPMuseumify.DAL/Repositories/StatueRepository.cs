using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMuseumify.DAL.Repositories;

public class StatueRepository : IStatueRepository
{
    private readonly ApplicationDbContext _context;

    public StatueRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Statue?> GetByIdAsync(Guid id)
    {
        return await _context.Statues.FindAsync(id);
    }

    public async Task<bool> UpdateVideoUrlAsync(Guid id, string videoUrl)
    {
        var statue = await _context.Statues.FindAsync(id);
        if (statue == null)
            return false;
        statue.VideoUrl = videoUrl;
        statue.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
