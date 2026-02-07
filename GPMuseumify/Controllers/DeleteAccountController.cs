using GPMuseumify.BL.DTOs.Auth;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPMuseumify.Controllers
{
    [ApiController]
    [Route("api/users/me")]
    [Authorize]
    public class DeleteAccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<DeleteAccountController> _logger;

        public DeleteAccountController(IAuthService authService, ILogger<DeleteAccountController> logger)
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

        /// <summary>حذف الحساب — يلزم الباسورد الحالي للتأكيد. الـ User وكل بياناته (History, Favorites) تتبدل.</summary>
        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required to confirm account deletion." });

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var success = await _authService.DeleteAccountAsync(userId.Value, dto);
            if (!success)
                return BadRequest(new { message = "Incorrect password." });

            return Ok(new { message = "Account deleted successfully. You can register again if you want to use the app." });
        }
    }
}