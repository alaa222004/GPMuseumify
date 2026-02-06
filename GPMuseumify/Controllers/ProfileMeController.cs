//using GPMuseumify.BL.DTOs.Profile;
//using GPMuseumify.DAL.Models;
//using GPMuseumify.DAL.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace GPMuseumify.Controllers;

//[ApiController]
//[Route("api/users/me/profile")]
//[Authorize]
//public class ProfileMeController : ControllerBase
//{
//    private readonly IUserRepository _userRepository;
//    private readonly ILogger<ProfileMeController> _logger;

//    public ProfileMeController(IUserRepository userRepository, ILogger<ProfileMeController> logger)
//    {
//        _userRepository = userRepository;
//        _logger = logger;
//    }

//    private Guid? GetCurrentUserId()
//    {
//        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
//            ?? User.FindFirst("sub")?.Value;
//        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
//            return null;
//        return userId;
//    }

//    /// <summary>جلب الاسم، الإيميل، رقم التلفون (اللي اتخزنوا وقت الـ Register ويظهرون في Personal Information).</summary>
//    [HttpGet]
//    public async Task<IActionResult> GetMyProfile()
//    {
//        var userId = GetCurrentUserId();
//        if (userId == null)
//            return Unauthorized();

//        var user = await _userRepository.GetByIdAsync(userId.Value);
//        if (user == null)
//            return NotFound(new { message = "User not found." });

//        return Ok(new UserProfileDto
//        {
//            UserId = user.Id,
//            Name = user.Name,
//            Email = user.Email,
//            PhoneNumber = user.PhoneNumber
//        });
//    }

//    /// <summary>تحديث الاسم و/أو رقم التلفون (زر Edit).</summary>
//    [HttpPut]
//    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto dto)
//    {
//        var userId = GetCurrentUserId();
//        if (userId == null)
//            return Unauthorized();

//        if (dto == null)
//            return BadRequest(new { message = "Request body is required." });

//        var user = await _userRepository.GetByIdAsync(userId.Value);
//        if (user == null)
//            return NotFound(new { message = "User not found." });

//        if (dto.Name != null)
//            user.Name = dto.Name.Trim();
//        if (dto.PhoneNumber != null)
//            user.PhoneNumber = dto.PhoneNumber.Trim();

//        await _userRepository.UpdateAsync(user);

//        return Ok(new UserProfileDto
//        {
//            UserId = user.Id,
//            Name = user.Name,
//            Email = user.Email,
//            PhoneNumber = user.PhoneNumber
//        });
//    }

//}using GPMuseumify.BL.DTOs.Profile;
using GPMuseumify.BL.DTOs.Profile;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPMuseumify.Controllers
{
    [ApiController]
    [Route("api/users/me/profile")]
    [Authorize] // Authenticate only, no roles required
    public class ProfileMeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProfileMeController> _logger;

        public ProfileMeController(IUserRepository userRepository, ILogger<ProfileMeController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get the current user's ID from JWT claims
        /// </summary>
        private Guid? GetCurrentUserId()
        {
            // Log all claims to debug
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
            }

            // Read userId directly from token claim "sub"
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return null;

            return userId;
        }

        /// <summary>
        /// Get the current user's profile (Name, Email, PhoneNumber)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "Invalid or expired token." });

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(new UserProfileDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
        }

        /// <summary>
        /// Update the current user's profile (Name and/or PhoneNumber)
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "Invalid or expired token." });

            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return NotFound(new { message = "User not found." });

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name.Trim();
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber.Trim();

            await _userRepository.UpdateAsync(user);

            return Ok(new UserProfileDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
        }
    }
}


