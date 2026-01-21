

using Google.Apis.Auth;
using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;


namespace GPMuseumify.BL.Services;


    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;// dependency injection داته بيز
    private readonly ITokenService _tokenService;// service 3mlto 3shan y3ml generate lel token
    private readonly IEmailService _emailService;// service 3mlto 3shan yb3t emails

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
                throw new InvalidOperationException("Email already exists");// lw el email mwgoda already barthrow exception
        }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);// hashing el password 3shan ma tkonsh mazbota bel clear text fel db

        // Generate verification code
        var verificationCode = GenerateVerificationCode();//OTP
        var verificationToken = Guid.NewGuid().ToString();// unique token

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

            user = await _userRepository.CreateAsync(user);// create user fel db

        // Send verification email
        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);// b3t email feh el OTP

        // Generate token
        var token = _tokenService.GenerateToken(user);// generate jwt token

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
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);// jlb el user by email
        if (user == null)
            {
                return null;
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))// verify el password elly edkhaltaha b el hashed password fel db
        {
                return null;
            }

            // Generate token
            var token = _tokenService.GenerateToken(user);// generate jwt token

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
                user.EmailVerificationExpiry < DateTime.UtcNow)// check if expired
        {
                return false;
            }

            // For now, we'll just verify if token exists (in production, verify the code)
            // TODO: Implement proper code verification
            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationExpiry = null;

            await _userRepository.UpdateAsync(user);// update el user fel db
        return true;
        }

        public async Task<bool> ResendVerificationCodeAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsEmailVerified)
            {
                return false;
            }

            var verificationCode = GenerateVerificationCode();// generate new OTP
        var verificationToken = Guid.NewGuid().ToString();// generate new unique token

        user.EmailVerificationToken = verificationCode;// set new OTP
        user.EmailVerificationExpiry = DateTime.UtcNow.AddHours(24);// set new expiry

        await _userRepository.UpdateAsync(user);// update el user fel db
        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);// b3t email feh el OTP

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
            return true; // Don't reveal if email exists or not (security best practice)

        // Generate numeric OTP for reset
        var resetCode = GenerateVerificationCode();// generate OTP
        user.ResetPasswordToken = resetCode;// set OTP
        user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);// set expiry

        await _userRepository.UpdateAsync(user);// update el user fel db
        await _emailService.SendPasswordResetEmailAsync(user.Email, resetCode);// b3t email feh el OTP

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
        var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);// jlb el user by reset token (OTP)
        if (user == null || user.ResetPasswordExpiry < DateTime.UtcNow)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);// hash el new password
        user.ResetPasswordToken = null;
        user.ResetPasswordExpiry = null;

        await _userRepository.UpdateAsync(user);// update el user fel db
        return true;
    }

    public async Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto)
        {
        return socialLoginDto.Provider.ToLower() switch // switch case 3la 7asab el provider
        {
            "google" => await GoogleLoginAsync(socialLoginDto.Token),// call google login method
            "apple" => await AppleLoginAsync(socialLoginDto.Token),// call apple login method
            _ => throw new InvalidOperationException("Unsupported provider")
        };
    }

    // -------- Google --------
    private async Task<AuthResponseDto?> GoogleLoginAsync(string idToken)// method to handle google login
    {
        GoogleJsonWebSignature.Payload payload;// to hold the payload from google

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken);// validate the id token and get the payload
        }
        catch
        {
            return null;
        }

        var email = payload.Email.ToLower();// get the email from the payload
        var name = payload.Name ?? "Google User";// get the name from the payload or set a default name

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)// if user not found, create a new user
        {
            user = new User
            {
                Email = email,
                Name = name,
                Role = "User",
                IsEmailVerified = true
            };

            user = await _userRepository.CreateAsync(user);// create the user in the db
        }

        var token = _tokenService.GenerateToken(user);// generate jwt token

        return new AuthResponseDto// return the auth response
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
    private async Task<AuthResponseDto?> AppleLoginAsync(string idToken)// method to handle apple login
    {
        var handler = new JwtSecurityTokenHandler();// to read the jwt token
        var jwtToken = handler.ReadJwtToken(idToken);// read the jwt token

        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;// get the email claim from the token
        if (string.IsNullOrEmpty(email))
            return null;

        email = email.ToLower();

        var user = await _userRepository.GetByEmailAsync(email);// jlb el user by email

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
    private string GenerateVerificationCode()// method to generate a 4-digit numeric OTP
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();// generate random number between 1000 and 9999
    }
}


