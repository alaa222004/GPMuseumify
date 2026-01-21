

using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.DTOs.Search;

public class SearchResponseDto
{
    public string Query { get; set; } = string.Empty;
    public List<SearchResultDto> Results { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new();

}
