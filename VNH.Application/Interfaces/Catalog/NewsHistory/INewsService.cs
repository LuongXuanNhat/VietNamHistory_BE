using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.NewsHistory
{
    public interface INewsService
    {
        Task<ApiResult<int>> CrawlNews(string url);
        Task<ApiResult<List<News>>> GetNews();
    }
}
