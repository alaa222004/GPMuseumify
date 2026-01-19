

using Google.Apis.Auth;
using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;


namespace GPMuseumify.BL.Services;


    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Generate verification code
            var verificationCode = GenerateVerificationCode();
            var verificationToken = Guid.NewGuid().ToString();

            // Validate Role (only allow Admin if provided, otherwise default to User)
            var role = registerDto.Role?.ToLower() == "admin" ? "Admin" : "User";

            // Create user
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email.ToLower(),
                PasswordHash = passwordHash,
                Role = role,
                EmailVerificationToken = verificationCode,//OTP
                EmailVerificationExpiry = DateTime.UtcNow.AddHours(24),
                IsEmailVerified = false
            };

            user = await _userRepository.CreateAsync(user);

            // Send verification email
            await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified
            };
        }

        public async Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
        {
            var user = await _userRepository.GetByEmailAsync(verifyEmailDto.Email);
            if (user == null)
            {
                return false;
            }

            // Check if already verified
            if (user.IsEmailVerified)
            {
                return true;
            }

            // Validate token and expiry
            if (string.IsNullOrEmpty(user.EmailVerificationToken) ||
                user.EmailVerificationExpiry == null ||
                user.EmailVerificationExpiry < DateTime.UtcNow)
            {
                return false;
            }

            // For now, we'll just verify if token exists (in production, verify the code)
            // TODO: Implement proper code verification
            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ResendVerificationCodeAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsEmailVerified)
            {
                return false;
            }

            var verificationCode = GenerateVerificationCode();
            var verificationToken = Guid.NewGuid().ToString();

            user.EmailVerificationToken = verificationCode;
            user.EmailVerificationExpiry = DateTime.UtcNow.AddHours(24);

            await _userRepository.UpdateAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

            return true;
        }

    //public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    //{
    //    var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
    //    if (user == null)
    //    {
    //        // Don't reveal if email exists or not (security best practice)
    //        return true;
    //    }

    //    var resetToken = Guid.NewGuid().ToString();
    //    user.ResetPasswordToken = resetToken;
    //    user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

    //    await _userRepository.UpdateAsync(user);
    //    await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);

    //    return true;
    //}

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
            return true; // Don't reveal if email exists

        // Generate numeric OTP for reset
        var resetCode = GenerateVerificationCode();
        user.ResetPasswordToken = resetCode;
        user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

        await _userRepository.UpdateAsync(user);
        await _emailService.SendPasswordResetEmailAsync(user.Email, resetCode);

        return true;
    }


    //public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    //    {
    //        var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);

    //        if (user == null)
    //        {
    //            return false;
    //        }

    //        // Hash new password
    //        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
    //        user.ResetPasswordToken = null;
    //        user.ResetPasswordExpiry = null;

    //        await _userRepository.UpdateAsync(user);
    //        return true;
    //    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);
        if (user == null || user.ResetPasswordExpiry < DateTime.UtcNow)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
        user.ResetPasswordToken = null;
        user.ResetPasswordExpiry = null;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto)
        {
        return socialLoginDto.Provider.ToLower() switch
        {
            "google" => await GoogleLoginAsync(socialLoginDto.Token),
            "apple" => await AppleLoginAsync(socialLoginDto.Token),
            _ => throw new InvalidOperationException("Unsupported provider")
        };
    }

    // -------- Google --------
    private async Task<AuthResponseDto?> GoogleLoginAsync(string idToken)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
        catch
        {
            return null;
        }

        var email = payload.Email.ToLower();
        var name = payload.Name ?? "Google User";

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                Email = email,
                Name = name,
                Role = "User",
                IsEmailVerified = true
            };

            user = await _userRepository.CreateAsync(user);
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = true
        };
    }

    // -------- Apple --------
    private async Task<AuthResponseDto?> AppleLoginAsync(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(idToken);

        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        if (string.IsNullOrEmpty(email))
            return null;

        email = email.ToLower();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                Email = email,
                Name = "Apple User",
                Role = "User",
                IsEmailVerified = true
            };

            user = await _userRepository.CreateAsync(user);
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = true
        };
    }

    // ================= UTIL =================
    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }
}


