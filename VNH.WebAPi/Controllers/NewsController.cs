using Microsoft.AspNetCore.Mvc;
using VNH.Application.Interfaces.Catalog.NewsHistory;
using VNH.Domain;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService) {
            _newsService = newsService;
        }
        [HttpPost]
        public async Task<IActionResult> CrawlNews(string? url = "https://danviet.vn/lich-su-viet-nam.html")
        {
            var result = await _newsService.CrawlNews(url);
            if (result.IsSuccessed)
            {
                result.Message = "Có " + result.ResultObj + " bản tin được cập nhập";
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            string url = "https://danviet.vn/lich-su-viet-nam.html";
            var newsList = await _newsService.GetNews(url);
            return Ok(newsList);
        }

    }
}
