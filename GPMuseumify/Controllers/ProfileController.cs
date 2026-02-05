using GPMuseumify.BL.DTOs.Profile;
using GPMuseumify.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers;

[ApiController]

[Route("api/users/{userId:guid}/profile")]

[Authorize(Policy = "UserOrAdmin")]

public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IUserRepository _userRepository;
    public ProfileController(ILogger<ProfileController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]

    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
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
    [HttpPut]

    public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UserProfileDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Request body is required." });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found." });

        if (dto.Name != null)
            user.Name = dto.Name.Trim();
        if (dto.PhoneNumber != null)
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




//using GPMuseumify.BL.DTOs.Profile;
//using GPMuseumify.DAL.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace GPMuseumify.Controllers;

//[ApiController]
//[Route("api/users/{userId:guid}/profile")]
//[Authorize(Policy = "UserOrAdmin")]
//public class ProfileController : ControllerBase
//{
//    private readonly ILogger<ProfileController> _logger;
//    private readonly IUserRepository _userRepository;

//    public ProfileController(ILogger<ProfileController> logger, IUserRepository userRepository)
//    {
//        _logger = logger;
//        _userRepository = userRepository;
//    }

//    // GET: api/users/{userId}/profile
//    [HttpGet]
//    public async Task<IActionResult> GetProfile(Guid userId)
//    {
//        // الحصول على بيانات المستخدم من التوكن
//        var tokenUserId = User.FindFirst("nameid")?.Value;
//        var role = User.FindFirst("role")?.Value;

//        // تحقق من صلاحيات الوصول
//        if (role != "Admin" && tokenUserId != userId.ToString())
//        {
//            return Forbid(); // أو return Unauthorized();
//        }

//        var user = await _userRepository.GetByIdAsync(userId);
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

//    // PUT: api/users/{userId}/profile
//    [HttpPut]
//    public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UserProfileDto dto)
//    {
//        if (dto == null)
//            return BadRequest(new { message = "Request body is required." });

//        // الحصول على بيانات المستخدم من التوكن
//        var tokenUserId = User.FindFirst("nameid")?.Value;
//        var role = User.FindFirst("role")?.Value;

//        // تحقق من صلاحيات التعديل
//        if (role != "Admin" && tokenUserId != userId.ToString())
//        {
//            return Forbid();
//        }

//        var user = await _userRepository.GetByIdAsync(userId);
//        if (user == null)
//            return NotFound(new { message = "User not found." });

//        if (!string.IsNullOrWhiteSpace(dto.Name))
//            user.Name = dto.Name.Trim();
//        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
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
//}



