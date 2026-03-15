
using GPMuseumify.BL.DTOs.History;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;

namespace GPMuseumify.BL.Services;

public class HistoryService : IHistoryService
{
    private const int MaxPageSize = 50;
    private readonly IUserHistoryRepository _userHistoryRepository; //byklm 3an el repository bta3 el user history
    public HistoryService(IUserHistoryRepository userHistoryRepository)
    {
        _userHistoryRepository = userHistoryRepository;
    }
   
       public async Task<UserHistoryItemDto> AddHistoryEntryAsync(Guid userId, CreateHistoryEntryDto dto)
    {
        if (dto.StatueId == null && dto.MuseumId == null)
        {
            throw new ArgumentException("Either StatueId or MuseumId must be provided.");
        }

        var history = new UserHistory
        {
            UserId = userId,
            StatueId = dto.StatueId,
            MuseumId = dto.MuseumId,
            SearchType = dto.SearchType,
            ViewedAt = dto.ViewedAt ?? DateTime.UtcNow
        };

        var created = await _userHistoryRepository.AddAsync(history);
        var createdWithDetails = await _userHistoryRepository.GetByIdWithDetailsAsync(created.Id) ?? created;

        return MapToDto(createdWithDetails);
    }

    private static UserHistoryItemDto MapToDto(UserHistory history)
    {
        var hasStatue = history.StatueId.HasValue;
        var hasMuseum = history.MuseumId.HasValue;

        var contentType = hasStatue ? "statue" : hasMuseum ? "museum" : "history";
        var title = history.Statue?.Name ?? history.Museum?.Name;
        var subtitle = history.Statue?.HistoricalPeriod ?? history.Museum?.Location;
        var description = history.Statue?.Description ?? history.Museum?.Description;
        var thumbnail = history.Statue?.ThumbnailUrl ?? history.Museum?.ImageUrl;
        var videoUrl = history.Statue?.VideoUrl;
        var videoUrlEn = history.Statue?.VideoUrlEn;

        return new UserHistoryItemDto
        {
            Id = history.Id,
            UserId = history.UserId,
            StatueId = history.StatueId,
            MuseumId = history.MuseumId,
            ContentType = contentType,
            Title = title,
            Subtitle = subtitle,
            Description = description,
            ThumbnailUrl = thumbnail,
            VideoUrl = videoUrl,
            VideoUrlEn = videoUrlEn,
            ViewedAt = history.ViewedAt,
            SearchType = history.SearchType
        };
    
    }

    public async Task<UserHistoryResponseDto> GetUserHistoryAsync(Guid userId, int page, int pageSize)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }
        else if (pageSize > MaxPageSize)
        {
            pageSize = MaxPageSize;
        }

        var skip = (page - 1) * pageSize;

        var historyItems = await _userHistoryRepository.GetUserHistoryAsync(userId, skip, pageSize);
        var totalItems = await _userHistoryRepository.CountByUserIdAsync(userId);
        return new UserHistoryResponseDto
        {
            UserId = userId,
            Items = historyItems.Select(MapToDto).ToList(),
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
            }
        };

    }
}
