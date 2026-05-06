

using GPMuseumify.BL.DTOs.Favorites;
using GPMuseumify.BL.DTOs.History;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Configuration;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;

namespace GPMuseumify.BL.Services;

public class FavoritesService: IFavoritesService
{
    private const int MaxPageSize = 50;
    private readonly IUserFavoriteRepository _userFavoriteRepository;

    public FavoritesService(IUserFavoriteRepository userFavoriteRepository)
    {
        _userFavoriteRepository = userFavoriteRepository;
    }

    public async Task<FavoritesResponseDto> GetUserFavoritesAsync(Guid userId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var skip = (page - 1) * pageSize;
        var items = await _userFavoriteRepository.GetUserFavoritesAsync(userId, skip, pageSize);
        var totalItems = await _userFavoriteRepository.CountByUserIdAsync(userId);

        return new FavoritesResponseDto
        {
            UserId = userId,
            Items = items.Select(MapToDto).ToList(),
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
            }
        };
    }

    public async Task<FavoriteItemDto?> AddFavoriteAsync(Guid userId, AddFavoriteDto dto)
    {
        if (dto.StatueId == null && dto.MuseumId == null)
            throw new ArgumentException("Either StatueId or MuseumId must be provided.");

        var favorite = new UserFavorite
        {
            UserId = userId,
            StatueId = dto.StatueId,
            MuseumId = dto.MuseumId
        };

        var created = await _userFavoriteRepository.AddAsync(favorite);
        if (created == null)
            return null;

        var withDetails = await _userFavoriteRepository.GetByIdAsync(created.Id);
        return withDetails != null ? MapToDto(withDetails) : null;
    }

    public async Task<bool> RemoveFavoriteAsync(Guid userId, Guid favoriteId)
    {
        var fav = await _userFavoriteRepository.GetByIdAsync(favoriteId);
        if (fav == null || fav.UserId != userId)
            return false;
        return await _userFavoriteRepository.RemoveAsync(favoriteId);
    }

    public async Task<bool> RemoveFavoriteByItemAsync(Guid userId, Guid? statueId, Guid? museumId)
    {
        var fav = await _userFavoriteRepository.GetByUserAndItemAsync(userId, statueId, museumId);
        if (fav == null)
            return false;
        return await _userFavoriteRepository.RemoveAsync(fav.Id);
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, Guid? statueId, Guid? museumId)
    {
        return await _userFavoriteRepository.ExistsAsync(userId, statueId, museumId);
    }

    private static FavoriteItemDto MapToDto(UserFavorite f)
    {
        var hasStatue = f.StatueId.HasValue;
        var contentType = hasStatue ? "statue" : "museum";
        var title = f.Statue?.Name ?? f.Museum?.Name;
        var titleAr = f.Statue?.NameAr ?? f.Museum?.NameAr;
        var subtitle = f.Statue?.HistoricalPeriod ?? f.Museum?.Location;
        var description = f.Statue?.Description ?? f.Museum?.Description;
        var thumbnail = f.Statue?.ThumbnailUrl ?? f.Museum?.ImageUrl;
        var imageUrl = f.Museum?.ImageUrl ?? f.Statue?.ThumbnailUrl;

        return new FavoriteItemDto
        {
            Id = f.Id,
            UserId = f.UserId,
            StatueId = f.StatueId,
            MuseumId = f.MuseumId,
            ContentType = contentType,
            Title = title,
            TitleAr = titleAr,
            Subtitle = subtitle,
            Description = description,
            ThumbnailUrl = thumbnail,
            ImageUrl = imageUrl,
            VideoUrl = f.Statue?.VideoUrl,
            VideoUrlEn = f.Statue?.VideoUrlEn,
            CreatedAt = f.CreatedAt
        };
    }

}
