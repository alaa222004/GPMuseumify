using GPMuseumify.BL.DTOs.History;
using GPMuseumify.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoryController : ControllerBase
{

    private readonly IHistoryService _historyService;
    private readonly ILogger _logger;

    public HistoryController(IHistoryService historyService, ILogger<HistoryController> logger)
    {
        _historyService = historyService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetHistory(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var response = await _historyService.GetUserHistoryAsync(userId, page, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching history for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to fetch history at this time." });
        }


    }
    [HttpPost]
    public async Task<IActionResult> AddHistory(Guid userId, [FromBody] CreateHistoryEntryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _historyService.AddHistoryEntryAsync(userId, dto);
            return CreatedAtAction(nameof(GetHistory), new { userId, page = 1, pageSize = 10 }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding history entry for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to add history entry at this time." });
        }
    }





}
