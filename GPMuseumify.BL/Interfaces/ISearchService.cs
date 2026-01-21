using GPMuseumify.BL.DTOs.Search;

namespace GPMuseumify.BL.Interfaces;

public interface ISearchService
{
    Task<SearchResponseDto>SearchAsync(SearchRequestDto request);
    Task<SuggestionsResponseDto> GetSuggestionsAsync(int statueCount = 5, int museumCount = 5);// el 7agat eli btzhr lw7dha f search bar

}
