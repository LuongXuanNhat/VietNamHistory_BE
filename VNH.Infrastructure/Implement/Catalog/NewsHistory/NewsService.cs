using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.NewsHistory;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.NewsHistory
{
    public class NewsService : INewsService
    {
        private readonly VietNamHistoryContext _dataContext;
        public NewsService(VietNamHistoryContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ApiResult<int>> CrawlNews(string url)
        {
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var newsList = new List<News>();
            try
            {
                var newsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='list-news']/li");
                if (newsNodes != null)
                {
                    foreach (var newsNode in newsNodes)
                    {
                        var titleNode = newsNode.SelectSingleNode(".//h3/a");
                        var descriptionNode = newsNode.SelectSingleNode(".//div[@class='sapo']");
                        var imageNode = newsNode.SelectSingleNode(".//img");
                        var pubDate = newsNode.SelectSingleNode(".//div[@class='catetime']//span[@class='time need-get-timeago']");

                        if (titleNode != null && descriptionNode != null && imageNode != null)
                        {
                            string titleAttributeValue = pubDate.GetAttributeValue("title", "");
                            var news = new News
                            {
                                Id = Guid.NewGuid(),
                                Title = titleNode.InnerText.Trim(),
                                Description = descriptionNode.InnerText.Trim(),
                                Image = imageNode.GetAttributeValue("src", "").Trim(),
                                Url = "https://danviet.vn" + titleNode.GetAttributeValue("href", "").Trim(),
                                CreatedAt = DateTime.Parse(titleAttributeValue)
                            };

                            newsList.Add(news);
                        }
                    }
                }
                int countUpdate = 0;
                newsList = newsList.OrderByDescending(x => x.CreatedAt).ToList();
                if (newsList.Count != 0)
                {
                    var news = await _dataContext.News.ToListAsync();
                    var article = news.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (news != null && article != null)
                    {
                        foreach (var item in newsList)
                        {
                            if (item.CreatedAt <= article.CreatedAt)
                            {
                                break;
                            }
                            countUpdate++;
                            _dataContext.News.Add(item);
                            await _dataContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        countUpdate = newsList.Count;
                        _dataContext.News.AddRange(newsList);
                        await _dataContext.SaveChangesAsync();
                    }
                }

                return new ApiSuccessResult<int>(countUpdate);

            }
            catch (Exception e)
            {
                return new ApiErrorResult<int>("Lỗi crawl: " + e.Message);
            }

        }

        public async Task<ApiResult<List<News>>> GetNews()
        {
            var news = await _dataContext.News.ToListAsync();
            news = news.OrderByDescending(news => news.CreatedAt).ToList();
            return new ApiSuccessResult<List<News>>(news);
        }
    }
}
