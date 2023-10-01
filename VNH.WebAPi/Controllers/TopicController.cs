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
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTopic(string topic)
        {
            var result = await _topicService.CreateTopic(topic, User.Identity.Name);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
