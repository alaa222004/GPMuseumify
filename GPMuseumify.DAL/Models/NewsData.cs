

namespace GPMuseumify.DAL.Models;

public class NewsData
{
    public List<EventItem> Events { get; set; } = new();
    public List<NewsItem> News { get; set; } = new();
}
