

namespace GPMuseumify.BL.DTOs.Search;

public class SuggestionsResponseDto
{
    public List<SearchResultDto> Statues { get; set; } = new();
    public List<SearchResultDto> Museums { get; set; } = new();
}
