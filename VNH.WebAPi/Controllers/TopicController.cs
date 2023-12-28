using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.Interfaces.Catalog.Topics;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService) {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopic()
        {
            var result = await _topicService.GetAllTopic();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string idTopic)
        {
            var result = await _topicService.GetById(idTopic);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTopic(string topic)
        {
            var result = await _topicService.CreateTopic(topic, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTopic(Guid topicId ,string topic)
        {
            var result = await _topicService.UpdateTopic(topicId, topic);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(Guid topicId )
        {
            var result = await _topicService.DeleteTopic(topicId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

    }
}
