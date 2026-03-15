using GPMuseumify.DAL.Models;

namespace GPMuseumify.DAL.Repositories;

public interface IStatueRepository
{
    Task<Statue?> GetByIdAsync(Guid id);
    Task<bool> UpdateVideoUrlAsync(Guid id, string videoUrl);
}
