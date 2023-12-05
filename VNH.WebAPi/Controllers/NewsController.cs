using Microsoft.AspNetCore.Mvc;
using VNH.Application.Interfaces.Catalog.NewsHistory;
using VNH.Domain;

namespace VNH.WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        NewsController(INewsService newsService) {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            string url = "https://danviet.vn/lich-su-viet-nam.html";
            List<News> newsList = await _newsService.GetNews(url);
            return Ok(newsList);
        }

    }
}
