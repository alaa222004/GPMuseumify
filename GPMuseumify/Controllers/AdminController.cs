using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GPMuseumify.DAL.Repositories;

namespace GPMuseumify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly IStatueRepository _statueRepository;

    public AdminController(IStatueRepository statueRepository)
    {
        _statueRepository = statueRepository;
    }

    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        return Ok(new { message = "Welcome to Admin Dashboard", role = "Admin" });
    }

    /// <summary>تحديث رابط فيديو التمثال (مثلاً لينك من Google Drive).</summary>
    [HttpPatch("statues/{statueId:guid}/video")]
    public async Task<IActionResult> UpdateStatueVideo(Guid statueId, [FromBody] UpdateStatueVideoRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.VideoUrl))
            return BadRequest(new { message = "VideoUrl is required." });
        if (request.VideoUrl.Length > 500)
            return BadRequest(new { message = "VideoUrl must be at most 500 characters." });

        var updated = await _statueRepository.UpdateVideoUrlAsync(statueId, request.VideoUrl.Trim());
        if (!updated)
            return NotFound(new { message = "Statue not found." });
        return Ok(new { message = "Video URL updated.", statueId });
    }
}

public class UpdateStatueVideoRequest
{
    public string VideoUrl { get; set; } = string.Empty;
}
