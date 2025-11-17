

using GPMuseumify.DAL.Models;

namespace GPMuseumify.BL.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
}
