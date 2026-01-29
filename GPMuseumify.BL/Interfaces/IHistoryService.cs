

using GPMuseumify.BL.DTOs.History;

namespace GPMuseumify.BL.Interfaces;

public interface IHistoryService
{
    Task<UserHistoryResponseDto> GetUserHistoryAsync(Guid userId, int page, int pageSize);
    Task <UserHistoryItemDto> AddHistoryEntryAsync(Guid userId, CreateHistoryEntryDto dto);
}
