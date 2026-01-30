using GPMuseumify.BL.DTOs.Settings;
using GPMuseumify.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers;

[ApiController]
[Route("api/users/{userId:guid}/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<SettingsController> _logger;
    public SettingsController(IUserRepository userRepository, ILogger<SettingsController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });

        }
        return Ok(new UserSettingsDto
        {
            PreferredLanguage = user.PreferredLanguage ?? "en",
            NotificationsEnabled = user.NotificationsEnabled
        });
    }
    [HttpPut]
    public async Task<IActionResult> UpdateSettings(Guid userId, [FromBody] UpdateUserSettingsDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Request body is required." });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found." });

        if (dto.PreferredLanguage != null)
            user.PreferredLanguage = dto.PreferredLanguage;
        if (dto.NotificationsEnabled.HasValue)
            user.NotificationsEnabled = dto.NotificationsEnabled.Value;

        await _userRepository.UpdateAsync(user);

        return Ok(new UserSettingsDto
        {
            PreferredLanguage = user.PreferredLanguage ?? "en",
            NotificationsEnabled = user.NotificationsEnabled
        });
    }

}







