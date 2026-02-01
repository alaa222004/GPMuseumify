

using GPMuseumify.BL.DTOs.News;
using GPMuseumify.BL.Interfaces;
using GPMuseumify.DAL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;

namespace GPMuseumify.BL.Services;



    public class NewsService : INewsService
    {
    private const int MaxPageSize = 50;
    private readonly ILogger<NewsService> _logger;
    private readonly string _jsonFilePath;
    private readonly ConcurrentDictionary<string, NewsData> _cache = new();
    private readonly FileSystemWatcher? _fileWatcher;
    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);

    public NewsService(ILogger<NewsService> logger, IHostEnvironment environment)
    {
        _logger = logger;

        // تحديد مسار ملف JSON
        // استخدام ContentRootPath إذا كان متاحاً، وإلا استخدام Directory.GetCurrentDirectory()
        var contentRoot = environment.ContentRootPath ?? Directory.GetCurrentDirectory();
        var dataPath = Path.Combine(contentRoot, "Data", "news.json");
        _jsonFilePath = dataPath;

        // تحميل البيانات عند البدء بشكل متزامن
        Task.Run(async () =>
        {
            await Task.Delay(500); // انتظار قصير للتأكد من أن كل شيء جاهز
            await LoadDataAsync();
        });

        // إعداد File Watcher لتحديث البيانات تلقائياً عند تغيير الملف
        try
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            var fileName = Path.GetFileName(_jsonFilePath);

            if (directory != null && Directory.Exists(directory))
            {
                _fileWatcher = new FileSystemWatcher(directory, fileName)
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                    EnableRaisingEvents = true
                };

                _fileWatcher.Changed += async (sender, e) =>
                {
                    try
                    {
                        // تأخير بسيط لتجنب قراءة الملف أثناء الكتابة
                        await Task.Delay(1000);
                        await LoadDataAsync();
                        _logger.LogInformation("News data automatically reloaded from JSON file after change detected");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error reloading news data after file change");
                    }
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not set up file watcher for news.json");
        }
    }

    private async Task LoadDataAsync()
    {
        await _loadSemaphore.WaitAsync();
        try
        {
            if (!File.Exists(_jsonFilePath))
            {
                _logger.LogWarning("News JSON file not found at {Path}", _jsonFilePath);
                return;
            }

            // محاولة قراءة الملف عدة مرات في حالة كان قيد الكتابة
            string jsonContent = string.Empty;
            int retries = 5;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
                    break;
                }
                catch (IOException)
                {
                    if (i < retries - 1)
                    {
                        await Task.Delay(100);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var data = JsonSerializer.Deserialize<NewsData>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data != null)
            {
                _cache["data"] = data;
                _logger.LogInformation("Loaded {NewsCount} news items and {EventCount} events from JSON",
                    data.News.Count, data.Events.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading news data from JSON file");
        }
        finally
        {
            _loadSemaphore.Release();
        }
    }

    private NewsData GetData()
    {
        if (_cache.TryGetValue("data", out var data))
        {
            return data;
        }

        // محاولة تحميل البيانات إذا لم تكن محملة
        Task.Run(async () => await LoadDataAsync()).Wait(TimeSpan.FromSeconds(5));

        return _cache.TryGetValue("data", out var loadedData)
            ? loadedData
            : new NewsData();
    }

    public async Task<NewsResponseDto> GetNewsAsync(int page = 1, int pageSize = 10, string? category = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var data = GetData();
        var news = data.News.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            news = news.Where(n =>
                n.Category != null &&
                n.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        var totalItems = news.Count();
        var skip = (page - 1) * pageSize;
        var pagedNews = news
            .OrderByDescending(n => n.PublishedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new NewsResponseDto
        {
            News = pagedNews.Select(MapToDto).ToList(),
            Pagination = new DTOs.History.PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
            }
        };
    }

    public async Task<EventsResponseDto> GetEventsAsync(int page = 1, int pageSize = 10, string? category = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var data = GetData();
        var events = data.Events.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            events = events.Where(e =>
                e.Category != null &&
                e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        var totalItems = events.Count();
        var skip = (page - 1) * pageSize;
        var pagedEvents = events
            .OrderBy(e => e.EventDate) // ترتيب حسب تاريخ الفعالية
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new EventsResponseDto
        {
            Events = pagedEvents.Select(MapToEventDto).ToList(),
            Pagination = new DTOs.History.PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
            }
        };
    }

    public async Task<AllContentResponseDto> GetAllContentAsync()
    {
        var data = GetData();

        return new AllContentResponseDto
        {
            News = data.News
                .OrderByDescending(n => n.PublishedAt)
                .Select(MapToDto)
                .ToList(),
            Events = data.Events
                .OrderBy(e => e.EventDate)
                .Select(MapToEventDto)
                .ToList()
        };
    }

    public async Task<NewsDto?> GetNewsByIdAsync(string id)
    {
        var data = GetData();
        var news = data.News.FirstOrDefault(n => n.Id == id);
        return news != null ? MapToDto(news) : null;
    }

    public async Task<EventDto?> GetEventByIdAsync(string id)
    {
        var data = GetData();
        var eventItem = data.Events.FirstOrDefault(e => e.Id == id);
        return eventItem != null ? MapToEventDto(eventItem) : null;
    }

    public async Task<List<NewsDto>> GetRecentNewsAsync(int count = 10)
    {
        var data = GetData();
        return data.News
            .OrderByDescending(n => n.PublishedAt)
            .Take(count)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<List<EventDto>> GetUpcomingEventsAsync(int count = 10)
    {
        var data = GetData();
        var now = DateTime.UtcNow;

        return data.Events
            .Where(e => e.EventDate >= now)
            .OrderBy(e => e.EventDate)
            .Take(count)
            .Select(MapToEventDto)
            .ToList();
    }

    public async Task ReloadDataAsync()
    {
        await LoadDataAsync();
    }

    private static NewsDto MapToDto(NewsItem news)
    {
        return new NewsDto
        {
            Id = news.Id,
            Title = news.Title,
            TitleAr = news.TitleAr,
            Description = news.Description,
            DescriptionAr = news.DescriptionAr,
            ImageUrl = news.ImageUrl,
            Category = news.Category,
            PublishedAt = news.PublishedAt,
            SourceName = news.SourceName
        };
    }

    private static EventDto MapToEventDto(EventItem eventItem)
    {
        return new EventDto
        {
            Id = eventItem.Id,
            Title = eventItem.Title,
            TitleAr = eventItem.TitleAr,
            Description = eventItem.Description,
            DescriptionAr = eventItem.DescriptionAr,
            ImageUrl = eventItem.ImageUrl,
            Category = eventItem.Category,
            EventDate = eventItem.EventDate,
            Location = eventItem.Location,
            LocationAr = eventItem.LocationAr,
            PublishedAt = eventItem.PublishedAt,
            SourceName = eventItem.SourceName
        };
    }
}




