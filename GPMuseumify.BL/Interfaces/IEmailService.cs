

namespace GPMuseumify.BL.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string code);
    Task SendPasswordResetEmailAsync(string email, string resetToken);
}
