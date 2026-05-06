using GPMuseumify.BL.DTOs.Search;

namespace GPMuseumify.BL.Interfaces;

public interface ISearchService
{
    Task<SearchResponseDto> SearchAsync(SearchRequestDto request);
    Task<SuggestionsResponseDto> GetSuggestionsAsync(int statueCount = 5, int museumCount = 5);
    /// <summary>جلب تفاصيل تمثال بالـ Id (للاستخدام بعد scan/upload عند التعرف على الصورة).</summary>
    Task<SearchResultDto?> GetStatueByIdAsync(Guid statueId);
    /// <summary>Language-aware lightweight details for a single statue (optimized for Flutter/clients).</summary>
    ///اااااااااا
    ///////////////////////
    Task<StatueDetailsDto?> GetStatueDetailsAsync(Guid id, string lang);
}
