

using GPMuseumify.BL.DTOs.Favorites;
using GPMuseumify.BL.DTOs.History;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using GPMuseumify.DAL.Repositories;


namespace GPMuseumify.BL.Services;

public class FavoritesNewsService:IFavoritesNewsService
{

    private const int MaxPageSize = 50;
    private readonly IUserFavoriteNewsRepository _repository;
    private readonly INewsService _newsService;

    public FavoritesNewsService(IUserFavoriteNewsRepository repository, INewsService newsService)
    {
        _repository = repository;
        _newsService = newsService;
    }

    public async Task<FavoritesNewsResponseDto> GetUserFavoriteNewsAsync(Guid userId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var skip = (page - 1) * pageSize;
        var favorites = await _repository.GetUserFavoriteNewsAsync(userId, skip, pageSize);
        var totalItems = await _repository.CountByUserIdAsync(userId);

        var items = new List<FavoriteNewsItemDto>();
        foreach (var f in favorites)
        {
            var dto = await MapToDtoAsync(f);
            if (dto != null)
                items.Add(dto);
        }

        return new FavoritesNewsResponseDto
        {
            UserId = userId,
            Items = items,
            Pagination = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
            }
        };
    }

    public async Task<FavoriteNewsItemDto?> AddFavoriteNewsAsync(Guid userId, AddFavoriteNewsDto dto)
    {
        var itemExists = dto.ItemType == "news"
            ? await _newsService.GetNewsByIdAsync(dto.ItemId) != null
            : await _newsService.GetEventByIdAsync(dto.ItemId) != null;
        if (!itemExists)
            throw new ArgumentException($"News or event with id '{dto.ItemId}' not found.");

        var favorite = new UserFavoriteNews
        {
            UserId = userId,
            ItemId = dto.ItemId,
            ItemType = dto.ItemType
        };

        var created = await _repository.AddAsync(favorite);
        if (created == null)
            return null;

        return await MapToDtoAsync(created);
    }

    public async Task<bool> RemoveFavoriteNewsAsync(Guid userId, string itemId ,string itemType)
    {
        return await _repository.RemoveAsync(userId, itemId,itemType);
    }






    public async Task<bool> IsFavoriteNewsAsync(Guid userId, string itemId, string itemType)
    {
        return await _repository.ExistsAsync(userId, itemId, itemType);
    }

    private async Task<FavoriteNewsItemDto?> MapToDtoAsync(UserFavoriteNews f)
    {
        if (f.ItemType == "news")
        {
            var news = await _newsService.GetNewsByIdAsync(f.ItemId);
            if (news == null) return null;
            return new FavoriteNewsItemDto
            {
                FavoriteId = f.Id,
                ItemId = f.ItemId,
                ItemType = "news",
                Title = news.Title,
                TitleAr = news.TitleAr,
                Description = news.Description,
                DescriptionAr = news.DescriptionAr,
                ImageUrl = news.ImageUrl,
                Category = news.Category,
                PublishedAt = news.PublishedAt,
                SourceName = news.SourceName,
                CreatedAt = f.CreatedAt
            };
        }

        var evt = await _newsService.GetEventByIdAsync(f.ItemId);
        if (evt == null) return null;
        return new FavoriteNewsItemDto
        {
            FavoriteId = f.Id,
            ItemId = f.ItemId,
            ItemType = "event",
            Title = evt.Title,
            TitleAr = evt.TitleAr,
            Description = evt.Description,
            DescriptionAr = evt.DescriptionAr,
            ImageUrl = evt.ImageUrl,
            Category = evt.Category,
            PublishedAt = evt.PublishedAt,
            EventDate = evt.EventDate,
            Location = evt.Location,
            LocationAr = evt.LocationAr,
            SourceName = evt.SourceName,
            CreatedAt = f.CreatedAt
        };
    }
}