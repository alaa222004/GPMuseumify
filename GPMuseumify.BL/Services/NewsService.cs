

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
        private readonly ILogger<NewsService> _logger;
        private readonly string _jsonFilePath;
        private readonly ConcurrentDictionary<string, NewsData> _cache = new();
        private readonly FileSystemWatcher? _fileWatcher;
        private readonly SemaphoreSlim _loadSemaphore = new(1, 1);

        public NewsService(ILogger<NewsService> logger, IHostEnvironment environment)
        {
            _logger = logger;

            // تحديد مسار ملف JSON
            var contentRoot = environment.ContentRootPath ?? Directory.GetCurrentDirectory();
            _jsonFilePath = Path.Combine(contentRoot, "Data", "news.json");

            // تحميل البيانات عند البدء
            Task.Run(async () =>
            {
                await Task.Delay(300); // انتظار قصير
                await LoadDataAsync();
            });

            // إعداد FileWatcher لمراقبة التغييرات
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

                    _fileWatcher.Changed += async (_, __) =>
                    {
                        await Task.Delay(500); // تجنب القراءة أثناء الكتابة
                        await LoadDataAsync();
                        _logger.LogInformation("news.json changed → data reloaded");
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "File watcher setup failed");
            }
        }

        // تحميل البيانات من JSON إلى الكاش
        private async Task LoadDataAsync()
        {
            await _loadSemaphore.WaitAsync();
            try
            {
                if (!File.Exists(_jsonFilePath))
                {
                    _logger.LogWarning("news.json not found at {Path}", _jsonFilePath);
                    return;
                }

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
                        if (i < retries - 1) await Task.Delay(100);
                        else throw;
                    }
                }

                var data = JsonSerializer.Deserialize<NewsData>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data != null)
                {
                    _cache["data"] = data;
                    _logger.LogInformation("Loaded {NewsCount} news items and {EventCount} events",
                        data.News.Count, data.Events.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading news.json");
            }
            finally
            {
                _loadSemaphore.Release();
            }
        }

        // الحصول على البيانات من الكاش
        private NewsData GetData()
        {
            if (_cache.TryGetValue("data", out var data))
                return data;

            LoadDataAsync().Wait(); // تحميل مؤقت لو الكاش فاضي
            return _cache.TryGetValue("data", out var loaded) ? loaded : new NewsData();
        }

        // ✅ الميثود الجديدة: كل الأخبار + الفعاليات
        public async Task<AllContentResponseDto> GetAllNewsAsync()
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

        // Reload يدوي
        public async Task ReloadDataAsync()
        {
            await LoadDataAsync();
        }

        // Mapping من Model إلى DTO
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

