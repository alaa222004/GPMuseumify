

using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Search;

public class SearchRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Query { get; set; } = string.Empty;
     public string? Type { get; set; } // "statue", "museum", or null for both
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
