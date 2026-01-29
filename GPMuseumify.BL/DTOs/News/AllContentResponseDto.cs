
using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.News;

public class AllContentResponseDto
{
    public List<NewsDto> News { get; set; }= new();// Initialize to avoid null reference issues
    public List<EventDto> Events { get; set; }= new();
}
