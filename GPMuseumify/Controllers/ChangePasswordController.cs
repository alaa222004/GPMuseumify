using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPMuseumify.Controllers;

[ApiController]
[Route("api/users/me")]
[Authorize]
public class ChangePasswordController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<ChangePasswordController> _logger;

    public ChangePasswordController(IAuthService authService, ILogger<ChangePasswordController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return null;
        return userId;
    }

    /// <summary>Update Password — يلزم Current Password و New Password و Confirm Password (مطابق لـ New Password).</summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.Compare(dto.NewPassword, dto.ConfirmPassword, StringComparison.Ordinal) != 0)
            return BadRequest(new { message = "Password and confirmation do not match." });

        if (string.Compare(dto.NewPassword, dto.CurrentPassword, StringComparison.Ordinal) == 0)
            return BadRequest(new { message = "New password must be different from current password." });

        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var success = await _authService.ChangePasswordAsync(userId.Value, dto);
        if (!success)
            return BadRequest(new { message = "Current password is incorrect." });

        return Ok(new { message = "Password updated successfully." });
    }
}


