using GPMuseumify.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace GPMuseumify.BL.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    // Verify Email (OTP)
    public async Task SendVerificationEmailAsync(string email, string code)
    {
        try
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@gpmuseumify.com";
            var fromName = _configuration["EmailSettings:FromName"] ?? "Echoes Of History";

            // لو الإعدادات مش موجودة
            if (string.IsNullOrEmpty(smtpHost) ||
                string.IsNullOrEmpty(smtpUsername) ||
                string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP settings not configured.");
                _logger.LogInformation($"Verification code for {email}: {code}");
                return;
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "Email Verification - Echoes Of History",
                Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 20px;'>
    <div style='max-width: 500px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
        <h2 style='color: #2c3e50; text-align: center;'>Echoes Of History</h2>
        <hr style='border:none;height:1px;background:#ddd;margin:20px 0;' />

        <p>Welcome to <strong>Echoes Of History</strong> 👋</p>
        <p>Please use the verification code below to confirm your email:</p>

        <div style='text-align:center;margin:30px 0;'>
            <span style='font-size:28px;letter-spacing:6px;font-weight:bold;color:#1abc9c;'>
                {code}
            </span>
        </div>

        <p style='color:#777;'>This code is valid for <strong>24 hours</strong>.</p>
        <p style='color:#777;'>If you did not create an account, please ignore this email.</p>

        <hr style='border:none;height:1px;background:#eee;margin:30px 0;' />
        <p style='font-size:12px;color:#aaa;text-align:center;'>
            © 2026 Echoes Of History. All rights reserved.
        </p>
    </div>
</body>
</html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Verification email sent to {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send verification email to {email}");
            _logger.LogInformation($"Verification code for {email}: {code}");
            throw;
        }
    }



    // Reset Password (OTP)
    public async Task SendPasswordResetEmailAsync(string email, string resetCode)
    {
        try
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@gpmuseumify.com";
            var fromName = _configuration["EmailSettings:FromName"] ?? "Echoes Of History";

            if (string.IsNullOrEmpty(smtpHost) ||
                string.IsNullOrEmpty(smtpUsername) ||
                string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP settings not configured.");
                _logger.LogInformation($"Password reset code for {email}: {resetCode}");
                return;
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

             var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "Reset Password - Echoes Of History",
                Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 20px;'>
    <div style='max-width: 500px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
        <h2 style='color:#c0392b;text-align:center;'>Password Reset</h2>
        <hr style='border:none;height:1px;background:#ddd;margin:20px 0;' />

        <p>You requested to reset your password.</p>
        <p>Use the following code to continue:</p>

        <div style='text-align:center;margin:30px 0;'>
            <span style='font-size:26px;letter-spacing:6px;font-weight:bold;color:#e74c3c;'>
                {resetCode}
            </span>
        </div>

        <p style='color:#777;'>This code is valid for <strong>1 hour</strong>.</p>
        <p style='color:#777;'>If you did not request this, please ignore this email.</p>

        <hr style='border:none;height:1px;background:#eee;margin:30px 0;' />
        <p style='font-size:12px;color:#aaa;text-align:center;'>
            © 2026 Echoes Of History. All rights reserved.
        </p>
    </div>
</body>
</html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Password reset email sent to {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send password reset email to {email}");
            _logger.LogInformation($"Password reset code for {email}: {resetCode}");
            throw;
        }
    }
}
