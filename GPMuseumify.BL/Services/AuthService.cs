

using Google.Apis.Auth;
using Microsoft.Extensions.Caching.Memory;
using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Ninject.Activation.Caching;
using Lucene.Net.Util.Cache;


namespace GPMuseumify.BL.Services;


//    public class AuthService : IAuthService
//    {
//        private readonly IUserRepository _userRepository;// dependency injection داته بيز
//    private readonly ITokenService _tokenService;// service 3mlto 3shan y3ml generate lel token
//    private readonly IEmailService _emailService;// service 3mlto 3shan yb3t emails

//    public AuthService(
//            IUserRepository userRepository,
//            ITokenService tokenService,
//            IEmailService emailService)
//        {
//            _userRepository = userRepository;
//            _tokenService = tokenService;
//            _emailService = emailService;
//        }

//        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
//        {
//            // Check if email already exists
//            if (await _userRepository.EmailExistsAsync(registerDto.Email))
//            {
//                throw new InvalidOperationException("Email already exists");// lw el email mwgoda already barthrow exception
//        }

//            // Hash password
//            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);// hashing el password 3shan ma tkonsh mazbota bel clear text fel db

//        // Generate verification code
//        var verificationCode = GenerateVerificationCode();//OTP
//        var verificationToken = Guid.NewGuid().ToString();// unique token

//        // Validate Role (only allow Admin if provided, otherwise default to User)
//        var role = registerDto.Role?.ToLower() == "admin" ? "Admin" : "User";

//            // Create user
//            var user = new User
//            {
//                Name = registerDto.Name,
//                Email = registerDto.Email.ToLower(),
//                PasswordHash = passwordHash,
//                Role = role,
//                EmailVerificationToken = verificationCode,//OTP
//                EmailVerificationExpiry = DateTime.UtcNow.AddHours(24),
//                IsEmailVerified = false
//            };

//            user = await _userRepository.CreateAsync(user);// create user fel db

//        // Send verification email
//        //await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);// b3t email feh el OTP

//        // Generate token
//        var token = _tokenService.GenerateToken(user);// generate jwt token

//        return new AuthResponseDto
//            {
//                UserId = user.Id,
//                Token = token,
//                Name = user.Name,
//                Email = user.Email,
//                Role = user.Role,
//                IsEmailVerified = user.IsEmailVerified
//            };
//        }

//        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
//        {
//            var user = await _userRepository.GetByEmailAsync(loginDto.Email);// jlb el user by email
//        if (user == null)
//            {
//                return null;
//            }

//            // Verify password
//            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))// verify el password elly edkhaltaha b el hashed password fel db
//        {
//                return null;
//            }
//        // لا يدخل التطبيق إلا بعد التحقق من الإيميل (OTP صحيح)
//        if (!user.IsEmailVerified)
//        {
//            throw new UnauthorizedAccessException("Please verify your email first. Check the code sent to your email.");
//        }


//        // Generate token
//        var token = _tokenService.GenerateToken(user);// generate jwt token

//        return new AuthResponseDto
//            {
//                UserId = user.Id,
//                Token = token,
//                Name = user.Name,
//                Email = user.Email,
//                Role = user.Role,
//                IsEmailVerified = user.IsEmailVerified
//            };
//        }

//    public async Task<AuthResponseDto?> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
//    {
//        var user = await _userRepository.GetByEmailAsync(verifyEmailDto.Email);
//        if (user == null)
//        {
//            return null;
//        }

//        // Check if already verified — نرجع token حتى يدخل التطبيق
//        if (user.IsEmailVerified)
//        {
//            var token = _tokenService.GenerateToken(user);
//            return new AuthResponseDto
//            {
//                UserId = user.Id,
//                Token = token,
//                Name = user.Name,
//                Email = user.Email,
//                Role = user.Role,
//                IsEmailVerified = true
//            };
//        }

//        // Validate token and expiry
//        if (string.IsNullOrEmpty(user.EmailVerificationToken) ||
//                user.EmailVerificationExpiry == null ||
//                user.EmailVerificationExpiry < DateTime.UtcNow)// check if expired
//        {
//                return null;
//            }

//        // For now, we'll just verify if token exists (in production, verify the code)
//        // TODO: Implement proper code verification
//        user.IsEmailVerified = true;
//        user.EmailVerificationToken = null;
//        user.EmailVerificationCode = null;
//        user.EmailVerificationExpiry = null;

//        await _userRepository.UpdateAsync(user);// update el user fel db
//                                                    // إرجاع Token بعد التحقق — المستخدم يدخل التطبيق مباشرة بدون Login
//        var newToken = _tokenService.GenerateToken(user);
//        return new AuthResponseDto
//        {
//            UserId = user.Id,
//            Token = newToken,
//            Name = user.Name,
//            Email = user.Email,
//            Role = user.Role,
//            IsEmailVerified = true
//        };
//    }

//        public async Task<bool> ResendVerificationCodeAsync(string email)
//        {
//            var user = await _userRepository.GetByEmailAsync(email);
//            if (user == null || user.IsEmailVerified)
//            {
//                return false;
//            }

//            var verificationCode = GenerateVerificationCode();// generate new OTP
//        var verificationToken = Guid.NewGuid().ToString();// generate new unique token

//        user.EmailVerificationToken = verificationCode;// set new OTP
//        user.EmailVerificationExpiry = DateTime.UtcNow.AddHours(24);// set new expiry

//        await _userRepository.UpdateAsync(user);// update el user fel db
//        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);// b3t email feh el OTP

//        return true;
//        }

//    //public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
//    //{
//    //    var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
//    //    if (user == null)
//    //    {
//    //        // Don't reveal if email exists or not (security best practice)
//    //        return true;
//    //    }

//    //    var resetToken = Guid.NewGuid().ToString();
//    //    user.ResetPasswordToken = resetToken;
//    //    user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

//    //    await _userRepository.UpdateAsync(user);
//    //    await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);

//    //    return true;
//    //}

//    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
//    {
//        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
//        if (user == null)
//            return true; // Don't reveal if email exists or not (security best practice)

//        // Generate numeric OTP for reset
//        var resetCode = GenerateVerificationCode();// generate OTP
//        user.ResetPasswordToken = resetCode;// set OTP
//        user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);// set expiry

//        await _userRepository.UpdateAsync(user);// update el user fel db
//        await _emailService.SendPasswordResetEmailAsync(user.Email, resetCode);// b3t email feh el OTP

//        return true;
//    }


//    //public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
//    //    {
//    //        var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);

//    //        if (user == null)
//    //        {
//    //            return false;
//    //        }

//    //        // Hash new password
//    //        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
//    //        user.ResetPasswordToken = null;
//    //        user.ResetPasswordExpiry = null;

//    //        await _userRepository.UpdateAsync(user);
//    //        return true;
//    //    }

//    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
//    {
//        var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);// jlb el user by reset token (OTP)
//        if (user == null || user.ResetPasswordExpiry < DateTime.UtcNow)
//            return false;

//        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);// hash el new password
//        user.ResetPasswordToken = null;
//        user.ResetPasswordExpiry = null;

//        await _userRepository.UpdateAsync(user);// update el user fel db
//        return true;
//    }

//    public async Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto)
//        {
//        return socialLoginDto.Provider.ToLower() switch // switch case 3la 7asab el provider
//        {
//            "google" => await GoogleLoginAsync(socialLoginDto.Token),// call google login method
//            "apple" => await AppleLoginAsync(socialLoginDto.Token),// call apple login method
//            _ => throw new InvalidOperationException("Unsupported provider")
//        };
//    }

//    // -------- Google --------
//    private async Task<AuthResponseDto?> GoogleLoginAsync(string idToken)// method to handle google login
//    {
//        GoogleJsonWebSignature.Payload payload;// to hold the payload from google

//        try
//        {
//            payload = await GoogleJsonWebSignature.ValidateAsync(idToken);// validate the id token and get the payload
//        }
//        catch
//        {
//            return null;
//        }

//        var email = payload.Email.ToLower();// get the email from the payload
//        var name = payload.Name ?? "Google User";// get the name from the payload or set a default name

//        var user = await _userRepository.GetByEmailAsync(email);

//        if (user == null)// if user not found, create a new user
//        {
//            user = new User
//            {
//                Email = email,
//                Name = name,
//                Role = "User",
//                IsEmailVerified = true
//            };

//            user = await _userRepository.CreateAsync(user);// create the user in the db
//        }

//        var token = _tokenService.GenerateToken(user);// generate jwt token

//        return new AuthResponseDto// return the auth response
//        {
//            UserId = user.Id,
//            Token = token,
//            Name = user.Name,
//            Email = user.Email,
//            Role = user.Role,
//            IsEmailVerified = true
//        };
//    }

//    // -------- Apple --------
//    private async Task<AuthResponseDto?> AppleLoginAsync(string idToken)// method to handle apple login
//    {
//        var handler = new JwtSecurityTokenHandler();// to read the jwt token
//        var jwtToken = handler.ReadJwtToken(idToken);// read the jwt token

//        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;// get the email claim from the token
//        if (string.IsNullOrEmpty(email))
//            return null;

//        email = email.ToLower();

//        var user = await _userRepository.GetByEmailAsync(email);// jlb el user by email

//        if (user == null)
//        {
//            user = new User
//            {
//                Email = email,
//                Name = "Apple User",
//                Role = "User",
//                IsEmailVerified = true
//            };

//            user = await _userRepository.CreateAsync(user);
//        }

//        var token = _tokenService.GenerateToken(user);

//        return new AuthResponseDto
//        {
//            UserId = user.Id,
//            Token = token,
//            Name = user.Name,
//            Email = user.Email,
//            Role = user.Role,
//            IsEmailVerified = true
//        };
//    }

//    // ================= UTIL =================
//    private string GenerateVerificationCode()// method to generate a 4-digit numeric OTP
//    {
//        var random = new Random();
//        return random.Next(1000, 9999).ToString();// generate random number between 1000 and 9999
//    }
//}


public class AuthService : IAuthService
{
    private const string ResetTokenCachePrefix = "reset:";
    private static readonly TimeSpan ResetTokenExpiry = TimeSpan.FromMinutes(10);
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IEmailService emailService,
         IConfiguration configuration,
            IMemoryCache cache)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
        _configuration = configuration;
        _cache = cache;
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
        //var role = registerDto.Role?.ToLower() == "admin" ? "Admin" : "User";

        // Create user
        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email.ToLower(),
            PasswordHash = passwordHash,
            //Role = role,
            PhoneNumber = registerDto.PhoneNumber?.Trim(),
            EmailVerificationToken = verificationToken,
            EmailVerificationCode = verificationCode,
            EmailVerificationExpiry = DateTime.UtcNow.AddHours(24),
            IsEmailVerified = false
        };

        user = await _userRepository.CreateAsync(user);

        // Generate verification link
        var verificationLink = $"{_configuration["AppSettings:BaseUrl"]}/api/auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={verificationToken}";

        // Send verification email
        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

        // لا نرجع Token — المستخدم لا يدخل التطبيق إلا بعد إدخال OTP الصحيح (verify-email)
        return new AuthResponseDto
        {
            UserId = user.Id,
            Token = null,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = false,
            //PhoneNumber = user.PhoneNumber,s

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

        // لا يدخل التطبيق إلا بعد التحقق من الإيميل (OTP صحيح)
        if (!user.IsEmailVerified)
        {
            throw new UnauthorizedAccessException("Please verify your email first. Check the code sent to your email.");
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

    public async Task<AuthResponseDto?> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
    {
        var user = await _userRepository.GetByEmailAsync(verifyEmailDto.Email);
        if (user == null)
        {
            return null;
        }

        // Check if already verified — نرجع token حتى يدخل التطبيق
        if (user.IsEmailVerified)
        {
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

        // Validate token and expiry
        if (string.IsNullOrEmpty(user.EmailVerificationToken) ||
            user.EmailVerificationExpiry == null ||
            user.EmailVerificationExpiry < DateTime.UtcNow)
        {
            return null;
        }

        // Verify the OTP code
        if (string.IsNullOrEmpty(user.EmailVerificationCode) ||
            user.EmailVerificationCode != verifyEmailDto.Code)
        {
            return null;
        }

        // Mark email as verified
        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationCode = null;
        user.EmailVerificationExpiry = null;

        await _userRepository.UpdateAsync(user);

        // إرجاع Token بعد التحقق — المستخدم يدخل التطبيق مباشرة بدون Login
        var newToken = _tokenService.GenerateToken(user);
        return new AuthResponseDto
        {
            UserId = user.Id,
            Token = newToken,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = true
        };
    }

    public async Task<bool> VerifyEmailByTokenAsync(string email, string token)
    {
        var user = await _userRepository.GetByEmailAsync(email);
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
            user.EmailVerificationToken != token ||
            user.EmailVerificationExpiry == null ||
            user.EmailVerificationExpiry < DateTime.UtcNow)
        {
            return false;
        }

        // Mark email as verified
        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationCode = null;
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
        user.EmailVerificationCode = verificationCode;
        user.EmailVerificationExpiry = DateTime.UtcNow.AddHours(24);

        await _userRepository.UpdateAsync(user);

        // Generate verification link
        var verificationLink = $"{_configuration["AppSettings:BaseUrl"]}/api/auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={verificationToken}";

        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

        return true;
    }

    //    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    //    {
    //        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
    //        if (user == null)
    //        {
    //            return true;
    //        }

    //        var otpCode = GenerateVerificationCode();
    //        user.ResetPasswordToken = otpCode;
    //        user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

    //        await _userRepository.UpdateAsync(user);
    //        await _emailService.SendPasswordResetEmailAsync(user.Email, otpCode);

    //        return true;
    //    }

    //    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    //    {
    //        var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email);
    //        if (user == null)
    //        {
    //            return false;
    //        }

    //        // Check expiry
    //        if (user.ResetPasswordExpiry == null || user.ResetPasswordExpiry < DateTime.UtcNow)
    //        {
    //            return false;
    //        }

    //        // Verify OTP code only (no token)
    //        if (string.IsNullOrEmpty(user.ResetPasswordToken) || user.ResetPasswordToken != resetPasswordDto.Code)
    //        {
    //            return false;
    //        }

    //        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
    //        user.ResetPasswordToken = null;
    //        user.ResetPasswordExpiry = null;

    //        await _userRepository.UpdateAsync(user);
    //        return true;
    //    }

    //    public async Task<AuthResponseDto?> SocialLoginAsync(SocialLoginDto socialLoginDto)
    //    {
    //        // TODO: Implement Google/Apple token validation
    //        // For now, return null (to be implemented)
    //        throw new NotImplementedException("Social login is not yet implemented");
    //    }

    //    private string GenerateVerificationCode()
    //    {
    //        var random = new Random();
    //        return random.Next(1000, 9999).ToString();
    //    }
    //}


    //public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    //{
    //    var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
    //    if (user == null)
    //    {
    //        return true;
    //    }

    //    var otpCode = GenerateVerificationCode();
    //    user.ResetPasswordToken = otpCode;
    //    user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);

    //    await _userRepository.UpdateAsync(user);
    //    await _emailService.SendPasswordResetEmailAsync(user.Email, otpCode);

    //    return true;
    //}

    //public async Task<bool> VerifyResetCodeAsync(VerifyResetCodeDto dto)
    //{
    //    var user = await _userRepository.GetByEmailAsync(dto.Email);
    //    if (user == null)
    //        return false;
    //    if (user.ResetPasswordExpiry == null || user.ResetPasswordExpiry < DateTime.UtcNow)
    //        return false;
    //    if (string.IsNullOrEmpty(user.ResetPasswordToken) || user.ResetPasswordToken != dto.Code)
    //        return false;
    //    return true;
    //}
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
            return true;

        var otpCode = GenerateVerificationCode();
        user.ResetPasswordToken = otpCode;
        user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateAsync(user);
        await _emailService.SendPasswordResetEmailAsync(user.Email, otpCode);

        return true;
    }

    public async Task<string?> VerifyResetCodeAsync(VerifyResetCodeDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            return null;
        if (user.ResetPasswordExpiry == null || user.ResetPasswordExpiry < DateTime.UtcNow)
            return null;
        if (string.IsNullOrEmpty(user.ResetPasswordToken) || user.ResetPasswordToken != dto.Code)
            return null;

        var resetToken = Guid.NewGuid().ToString("N");
        _cache.Set(ResetTokenCachePrefix + resetToken, user.Email, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = ResetTokenExpiry });
        return resetToken;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        if (!_cache.TryGetValue(ResetTokenCachePrefix + resetPasswordDto.Token, out string? email) || string.IsNullOrEmpty(email))
            return false;

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
        user.ResetPasswordToken = null;
        user.ResetPasswordExpiry = null;
        await _userRepository.UpdateAsync(user);

        _cache.Remove(ResetTokenCachePrefix + resetPasswordDto.Token);
        return true;
    }
    //public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    //{
    //    var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email);
    //    if (user == null)
    //        return false;
    //    if (user.ResetPasswordExpiry == null || user.ResetPasswordExpiry < DateTime.UtcNow)
    //        return false;
    //    if (string.IsNullOrEmpty(user.ResetPasswordToken) || user.ResetPasswordToken != resetPasswordDto.Code)
    //        return false;

    //    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
    //    user.ResetPasswordToken = null;
    //    user.ResetPasswordExpiry = null;

    //    await _userRepository.UpdateAsync(user);
    //    return true;
    //}


    //public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    //{
    //    var user = await _userRepository.GetByResetTokenAsync(resetPasswordDto.Token);

    //    if (user == null)

    //    {
    //        return false;
    //    }

    //    // Hash new password
    //    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
    //    user.ResetPasswordToken = null;
    //    user.ResetPasswordExpiry = null;

    //    await _userRepository.UpdateAsync(user);
    //    return true;
    //}

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
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

