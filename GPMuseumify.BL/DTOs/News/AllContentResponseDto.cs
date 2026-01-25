
using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.News;

public class AllContentResponseDto
{
    public List<NewsDto> News { get; set; }= new();
    public List<EventDto> Events { get; set; }= new();
}
