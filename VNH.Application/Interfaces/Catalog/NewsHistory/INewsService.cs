using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.NewsHistory
{
    public interface INewsService
    {
        Task<List<News>> GetNews(string url);
    }
}
