

using GPMuseumify.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GPMuseumify.BL.Services;

public class EmailService: IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendVerificationEmailAsync(string email, string code)
    {
        // TODO: Implement actual email sending (SendGrid, SMTP, etc.)
        // For now, just log it
        _logger.LogInformation($"Verification code for {email}: {code}");

        // In production, use SendGrid or similar:
        // var apiKey = _configuration["EmailSettings:SendGridApiKey"];
        // ... send email logic
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        // TODO: Implement actual email sending
        var resetUrl = $"{_configuration["AppSettings:BaseUrl"]}/reset-password?token={resetToken}";
        _logger.LogInformation($"Password reset link for {email}: {resetUrl}");

        // In production, use SendGrid or similar:
        // ... send email logic
    }

}
