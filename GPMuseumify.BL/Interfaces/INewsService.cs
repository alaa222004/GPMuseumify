
using GPMuseumify.BL.DTOs.News;

namespace GPMuseumify.BL.Interfaces;

public interface INewsService
{
    Task<AllContentResponseDto> GetAllNewsAsync();
    Task ReloadDataAsync();

}
