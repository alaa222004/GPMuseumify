
using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.News;

public class NewsResponseDto
{
    public List<NewsDto> News { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new();
}

public class EventsResponseDto
{
    public List<EventDto> Events { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new();
}

