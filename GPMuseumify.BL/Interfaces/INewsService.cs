
using GPMuseumify.BL.DTOs.News;

namespace GPMuseumify.BL.Interfaces;

public interface INewsService
{
    Task<NewsResponseDto> GetNewsAsync(int page = 1, int pageSize = 10, string? category = null);
    Task<EventsResponseDto> GetEventsAsync(int page = 1, int pageSize = 10, string? category = null);
    Task<AllContentResponseDto> GetAllContentAsync();
    Task<NewsDto?> GetNewsByIdAsync(string id);
    Task<EventDto?> GetEventByIdAsync(string id);
    Task<List<NewsDto>> GetRecentNewsAsync(int count = 10);
    Task<List<EventDto>> GetUpcomingEventsAsync(int count = 10);
    Task ReloadDataAsync();
}
