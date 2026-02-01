

namespace GPMuseumify.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GPMuseumify.BL.DTOs.Favorites;
using GPMuseumify.BL.Interfaces;
[ApiController]
[Route("api/users/{userId:guid}/favorites")]
[Authorize(Policy = "UserOrAdmin")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoritesService _favoritesService;
    private readonly IFavoritesNewsService _favoritesNewsService;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(IFavoritesService favoritesService, IFavoritesNewsService favoritesNewsService, ILogger<FavoritesController> logger)
    {
        _favoritesService = favoritesService;
        _favoritesNewsService = favoritesNewsService;
        _logger = logger;
    }

    // ---------- Favorites > Statue & Museums ----------

    /// <summary>قائمة المفضلة (تماثيل + متاحف) — تاب Statue / Museums.</summary>
    [HttpGet]
    public async Task<IActionResult> GetFavorites(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var response = await _favoritesService.GetUserFavoritesAsync(userId, page, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching favorites for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to fetch favorites." });
        }
    }

    /// <summary>هل التمثال/المتحف مفضّل؟ (قلب أحمر).</summary>
    [HttpGet("check")]
    public async Task<IActionResult> IsFavorite(Guid userId, [FromQuery] Guid? statueId, [FromQuery] Guid? museumId)
    {
        if (!statueId.HasValue && !museumId.HasValue)
            return BadRequest(new { message = "Provide statueId or museumId." });

        var isFav = await _favoritesService.IsFavoriteAsync(userId, statueId, museumId);
        return Ok(new { isFavorite = isFav });
    }

    /// <summary>إضافة تمثال أو متحف للمفضلة.</summary>
    [HttpPost]
    public async Task<IActionResult> AddFavorite(Guid userId, [FromBody] AddFavoriteDto dto)
    {
        if (dto == null || (dto.StatueId == null && dto.MuseumId == null))
            return BadRequest(new { message = "Provide StatueId or MuseumId." });

        try
        {
            var item = await _favoritesService.AddFavoriteAsync(userId, dto);
            if (item == null)
                return Ok(new { message = "Already in favorites.", isFavorite = true });
            return CreatedAtAction(nameof(GetFavorites), new { userId }, item);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding favorite for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to add favorite." });
        }
    }

    /// <summary>إزالة من المفضلة بالـ favorite id.</summary>
    [HttpDelete("{favoriteId:guid}")]
    public async Task<IActionResult> RemoveFavorite(Guid userId, Guid favoriteId)
    {
        var removed = await _favoritesService.RemoveFavoriteAsync(userId, favoriteId);
        if (!removed)
            return NotFound(new { message = "Favorite not found." });
        return Ok(new { message = "Removed from favorites." });
    }

    /// <summary>إزالة من المفضلة بـ statueId أو museumId.</summary>
    [HttpDelete("by-item")]
    public async Task<IActionResult> RemoveFavoriteByItem(Guid userId, [FromQuery] Guid? statueId, [FromQuery] Guid? museumId)
    {
        if (!statueId.HasValue && !museumId.HasValue)
            return BadRequest(new { message = "Provide statueId or museumId." });

        var removed = await _favoritesService.RemoveFavoriteByItemAsync(userId, statueId, museumId);
        if (!removed)
            return NotFound(new { message = "Favorite not found." });
        return Ok(new { message = "Removed from favorites." });
    }

    // ---------- Favorites > News ----------

    /// <summary>قائمة المفضلة (أخبار + أحداث) — تاب News.</summary>
    [HttpGet("news")]
    public async Task<IActionResult> GetFavoriteNews(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var response = await _favoritesNewsService.GetUserFavoriteNewsAsync(userId, page, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching favorite news for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to fetch favorite news." });
        }
    }

    /// <summary>هل الخبر/الحدث مفضّل؟ (قلب أحمر على News).</summary>
    [HttpGet("news/check")]
    public async Task<IActionResult> IsFavoriteNews(Guid userId, [FromQuery] string itemId, [FromQuery] string itemType = "news")
    {
        if (string.IsNullOrEmpty(itemId))
            return BadRequest(new { message = "Provide itemId." });
        if (itemType != "news" && itemType != "event")
            return BadRequest(new { message = "itemType must be 'news' or 'event'." });

        var isFav = await _favoritesNewsService.IsFavoriteNewsAsync(userId, itemId, itemType);
        return Ok(new { isFavorite = isFav });
    }

    /// <summary>إضافة خبر أو حدث للمفضلة.</summary>
    [HttpPost("news")]
    public async Task<IActionResult> AddFavoriteNews(Guid userId, [FromBody] AddFavoriteNewsDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.ItemId))
            return BadRequest(new { message = "Provide itemId and itemType." });

        try
        {
            var item = await _favoritesNewsService.AddFavoriteNewsAsync(userId, dto);
            if (item == null)
                return Ok(new { message = "Already in favorites.", isFavorite = true });
            return CreatedAtAction(nameof(GetFavoriteNews), new { userId }, item);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding favorite news for user {UserId}", userId);
            return StatusCode(500, new { message = "Unable to add favorite news." });
        }
    }

    /// <summary>إزالة خبر/حدث من المفضلة.</summary>
    [HttpDelete("news/by-item")]
    public async Task<IActionResult> RemoveFavoriteNewsByItem(Guid userId, [FromQuery] string itemId, [FromQuery] string itemType = "news")
    {
        if (string.IsNullOrEmpty(itemId))
            return BadRequest(new { message = "Provide itemId." });
        if (itemType != "news" && itemType != "event")
            return BadRequest(new { message = "itemType must be 'news' or 'event'." });

        var removed = await _favoritesNewsService.RemoveFavoriteNewsAsync(userId, itemId, itemType);
        if (!removed)
            return NotFound(new { message = "Favorite not found." });
        return Ok(new { message = "Removed from favorites." });
    }
}