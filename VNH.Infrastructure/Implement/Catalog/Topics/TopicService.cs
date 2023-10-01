using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Topics;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.Accounts;
using VNH.Application.Interfaces.Catalog.Topics;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Topics
{
    public class TopicService : ITopicService
    {
        private readonly VietNamHistoryContext _dbContext;
        private readonly UserManager<User> _userManager;

        public TopicService(VietNamHistoryContext dbContext, UserManager<User> account) { 
            _dbContext = dbContext;
            _userManager = account;
        }
        public async Task<ApiResult<List<TopicReponseDto>>> CreateTopic(string topicName, string userEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                var createTopic = new Topic()
                {
                    Id = Guid.NewGuid(),
                    Title = topicName,
                    CreatedAt = DateTime.UtcNow,
                    AuthorId = user.Id,
                };
                await _dbContext.Topics.AddAsync(createTopic);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ApiErrorResult<List<TopicReponseDto>>("Lỗi thêm chủ đề");
            }
            return await GetAllTopic();
        }

        public async Task<ApiResult<bool>> DeleteTopic(Guid topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<TopicReponseDto>>> GetAllTopic()
        {
            var topics = await _dbContext.Topics.ToListAsync();
            var topicResponse = new List<TopicReponseDto>();
            foreach (var item in topics)
            {
                var topic = new TopicReponseDto();
                topic.Id = item.Id;
                topic.Title = item.Title;
                topicResponse.Add(topic);
            }
            return new ApiSuccessResult<List<TopicReponseDto>>(topicResponse);
        }

        public async Task<ApiResult<bool>> GetTopic(Guid topicId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> UpdateTopic(Guid topicId, string topic)
        {
            throw new NotImplementedException();
        }
    }
}
