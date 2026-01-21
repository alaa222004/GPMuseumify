using GPMuseumify.BL.DTOs.Search;
using GPMuseumify.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers;


[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ISearchService searchService, ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromBody] SearchRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _searchService.SearchAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing search with query: {Query}", request.Query);
            return StatusCode(500, new { message = "Unable to perform search at this time." });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SearchGet([FromQuery] string query, [FromQuery] string? type = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { message = "Query parameter is required." });
        }

        var request = new SearchRequestDto
        {
            Query = query,
            Type = type,
            Page = page,
            PageSize = pageSize
        };

        try
        {
            var response = await _searchService.SearchAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing search with query: {Query}", query);
            return StatusCode(500, new { message = "Unable to perform search at this time." });
        }
    }

    [HttpGet("suggestions")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSuggestions([FromQuery] int statueCount = 5, [FromQuery] int museumCount = 5)
    {
        try
        {
            var response = await _searchService.GetSuggestionsAsync(statueCount, museumCount);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching suggestions");
            return StatusCode(500, new { message = "Unable to fetch suggestions at this time." });
        }
    }
}



