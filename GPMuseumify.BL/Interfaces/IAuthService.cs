

using GPMuseumify.BL.DTOs.Auth;

namespace GPMuseumify.BL.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    //Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<AuthResponseDto?> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<bool> ResendVerificationCodeAsync(string email);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);

    Task<string> VerifyResetCodeAsync(VerifyResetCodeDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    Task<bool> DeleteAccountAsync(Guid userId, DeleteAccountDto dto);
    Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto);

}
