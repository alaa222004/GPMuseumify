using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration: {Message}", ex.Message);
            var errorMessage = "An error occurred during registration";

            // Include exception details in development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                errorMessage = $"An error occurred during registration: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner: {ex.InnerException.Message}";
                }
            }

            return StatusCode(500, new { message = errorMessage });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.VerifyEmailAsync(verifyEmailDto);
            if (!result)
            {
                return BadRequest(new { message = "Invalid or expired verification code" });
            }

            return Ok(new { message = "Email verified successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during email verification");
            return StatusCode(500, new { message = "An error occurred during email verification" });
        }
    }

    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerification([FromBody] ForgotPasswordDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ResendVerificationCodeAsync(dto.Email);
            if (!result)
            {
                return BadRequest(new { message = "Email not found or already verified" });
            }

            return Ok(new { message = "Verification code sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resending verification code");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.ForgotPasswordAsync(forgotPasswordDto);
            // Always return success for security (don't reveal if email exists)
            return Ok(new { message = "If the email exists, a password reset link has been sent" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            if (!result)
            {
                return BadRequest(new { message = "Invalid or expired reset token" });
            }

            return Ok(new { message = "Password reset successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return StatusCode(500, new { message = "An error occurred during password reset" });
        }
    }

    [HttpPost("social-login")]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginDto socialLoginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.SocialLoginAsync(socialLoginDto);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid social login credentials" });
            }

            return Ok(result);
        }
        catch (NotImplementedException)
        {
            return StatusCode(501, new { message = "Social login is not yet implemented" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during social login");
            return StatusCode(500, new { message = "An error occurred during social login" });
        }
    }
}