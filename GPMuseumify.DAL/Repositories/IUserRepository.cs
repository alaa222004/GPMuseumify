

using GPMuseumify.DAL.Models;

namespace GPMuseumify.DAL.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByResetTokenAsync(string resetToken);
}
