
using GPMuseumify.BL.DTOs.Search;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Repositories;
using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.Services;

public class SearchService : ISearchService
{
    private const int MaxPageSize = 50; //3shan ma yb2ash fyh overload 3ala el database
    private readonly ISearchRepository _searchRepository; // Dependency Injection of the repository
    public SearchService(ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
   public async Task<SearchResponseDto> SearchAsync(SearchRequestDto request)
    {
        if (request.Page < 1)
        {
            request.Page = 1;
        }
        if (request.PageSize < 1)
        {
            request.PageSize = 10;
        }
        else if (request.PageSize > MaxPageSize)
        {
            request.PageSize = MaxPageSize;
        }

        var skip = (request.Page - 1) * request.PageSize; //by7sb el skip 3lshan a3ml pagination
        var results = new List<SearchResultDto>(); //by7ot feh el results eli 3mltha fetch mn el database
        int totalItems = 0; //by7sb feh el total items eli la2etha 3la el query

        if (string.IsNullOrWhiteSpace(request.Type) || request.Type.Equals("statue", StringComparison.OrdinalIgnoreCase))
        {
            var statues = await _searchRepository.SearchStatuesAsync(request.Query, skip, request.PageSize);// by3ml fetch l statues mn el database
            var statueCount = await _searchRepository.CountStatuesAsync(request.Query);// by7sb kam statue la2etha 3la el query
            results.AddRange(statues.Select(MapStatueToDto));
            totalItems += statueCount;

        }
        if (string.IsNullOrWhiteSpace(request.Type) || request.Type.Equals("museum", StringComparison.OrdinalIgnoreCase))
        {
            var museums = await _searchRepository.SearchMuseumsAsync(request.Query, skip, request.PageSize);
            var museumCount = await _searchRepository.CountMuseumsAsync(request.Query);
            results.AddRange(museums.Select(MapMuseumToDto));
            totalItems += museumCount;
        }
        return new SearchResponseDto
        {
            Results = results,
            Query = request.Query,
            Pagination = new PaginationDto
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)System.Math.Ceiling(totalItems / (double)request.PageSize)
            }
        };
    }
    public async Task<SuggestionsResponseDto> GetSuggestionsAsync(int statueCount = 5, int museumCount = 5)// el 7agat eli btzhr lw7dha f search bar
    {
        var statues = await _searchRepository.GetPopularStatuesAsync(statueCount);// by3ml fetch l statues mn el database
        var museums = await _searchRepository.GetPopularMuseumsAsync(museumCount);// by3ml fetch l museums mn el database

        return new SuggestionsResponseDto// byrg3 el results
        {
            Statues = statues.Select(MapStatueToDto).ToList(),
            Museums = museums.Select(MapMuseumToDto).ToList()
        };
    }

    private static SearchResultDto MapStatueToDto(DAL.Models.Statue statue)// by7wl el statue ll dto
    {
        return new SearchResultDto
        {
            Id = statue.Id,
            Name = statue.Name,
            NameAr = statue.NameAr,
            Description = statue.Description,
            DescriptionAr = statue.DescriptionAr,
            Location = statue.Location,
            ThumbnailUrl = statue.ThumbnailUrl,
            Type = "statue",
            HistoricalPeriod = statue.HistoricalPeriod,
            Museum = statue.Museum
        };
    }

    private static SearchResultDto MapMuseumToDto(DAL.Models.Museum museum)// by7wl el museum ll dto
    {
        return new SearchResultDto
        {
            Id = museum.Id,
            Name = museum.Name,
            NameAr = museum.NameAr,
            Description = museum.Description,
            Location = museum.Location,
            ImageUrl = museum.ImageUrl,
            Type = "museum"
        };
    }

}

