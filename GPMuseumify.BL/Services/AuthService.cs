

using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;
using Org.BouncyCastle.Crypto.Generators;

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
                EmailVerificationToken = verificationToken,
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

            user.EmailVerificationToken = verificationToken;
            user.EmailVerificationExpiry = DateTime.UtcNow.AddHours(24);

            await _userRepository.UpdateAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                // Don't reveal if email exists or not (security best practice)
                return true;
            }

            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateAsync(user);
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);

            if (user == null)
            {
                return false;
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto)
        {
            // TODO: Implement Google/Apple token validation
            // For now, return null (to be implemented)
            throw new NotImplementedException("Social login is not yet implemented");
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

    }

