using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.Interfaces.Catalog.NewsHistory;
using VNH.Domain;

namespace VNH.Infrastructure.Implement.Catalog.NewsHistory
{
    public class NewsService : INewsService
    {

        public async Task<List<News>> GetNews(string url)
        {
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var newsList = new List<News>();

            var newsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='list-news']/li");
            if (newsNodes != null)
            {
                foreach (var newsNode in newsNodes)
                {
                    var titleNode = newsNode.SelectSingleNode(".//h3/a");
                    var descriptionNode = newsNode.SelectSingleNode(".//div[@class='sapo']");
                    var imageNode = newsNode.SelectSingleNode(".//img");

                    if (titleNode != null && descriptionNode != null && imageNode != null)
                    {
                        var news = new News
                        {
                            Id = Guid.NewGuid(),
                            Title = titleNode.InnerText.Trim(),
                            Description = descriptionNode.InnerText.Trim(),
                            Image = imageNode.GetAttributeValue("src", "").Trim(),
                            Url = titleNode.GetAttributeValue("href", "").Trim()
                        };

                        newsList.Add(news);
                    }
                }
            }

            return newsList;
        }
    }
}
