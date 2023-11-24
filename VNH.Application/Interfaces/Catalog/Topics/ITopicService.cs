using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Topics;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Catalog.Topics
{
    public interface ITopicService
    {
        Task<ApiResult<List<TopicReponseDto>>> GetAllTopic();
        Task<ApiResult<bool>> DeleteTopic(Guid topicId);
        Task<ApiResult<List<TopicReponseDto>>> CreateTopic(string topic, string userEmail);
        Task<ApiResult<bool>> UpdateTopic(Guid topicId, string topic);
        Task<ApiResult<TopicReponseDto>> GetById(string idTopic);
    }
}
