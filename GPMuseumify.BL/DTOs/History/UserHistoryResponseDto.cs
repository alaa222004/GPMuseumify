

namespace GPMuseumify.BL.DTOs.History;

public class UserHistoryResponseDto
{

    public Guid UserId { get; set; }
    public List<UserHistoryItemDto> Items { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new();
}

public class PaginationDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasMore => Page < TotalPages;
}

