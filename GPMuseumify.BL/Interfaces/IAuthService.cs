

using GPMuseumify.BL.DTOs.Auth;

namespace GPMuseumify.BL.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<bool> ResendVerificationCodeAsync(string email);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto);

}
