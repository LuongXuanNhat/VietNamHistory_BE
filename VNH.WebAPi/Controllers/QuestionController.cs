using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.Forum;
using VNH.Application.Interfaces.Posts;
using VNH.Infrastructure.Implement.Catalog.Posts;

namespace VNH.WebAPi.Controllers
{

    [Route("Question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService; 
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateQuestion([FromForm] CreateQuestionDto requestDto)
        {
            var result = await _questionService.Create(requestDto, User.Identity.Name);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateQuestion([FromForm] CreateQuestionDto requestDto)
        {
            var result = await _questionService.Update(requestDto, User.Identity.Name);
            return result == null ? BadRequest(result) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _questionService.Detail(id);
            if(result.IsSuccessed)
                return Ok(result);
            return  BadRequest(result);
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> SubDetail(string subId)
        {
            var result = await _questionService.SubDetail(subId);
            if(result.IsSuccessed)
                return Ok(result);
            return  BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _questionService.GetAll();
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _questionService.Delete(Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return result is null ? BadRequest(result) : Ok(result);
        }


        [HttpGet("Save")]
        [Authorize]
        public async Task<IActionResult> GetSaveQuestion([FromForm] QuestionFpkDto questionFpk)
        {
            var result = await _questionService.GetSave(questionFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] QuestionFpkDto questionFpk)
        {
            var result = await _questionService.AddOrRemoveSaveQuestion(questionFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }


        [HttpGet("AllTag")]
        public async Task<IActionResult> GetAllTag( int numberTag)
        {
            var result = await _questionService.GetAllTag(numberTag);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpGet("FindByTag")]
        public async Task<IActionResult> GetQuestionByTag(string tag)
        {
            var result = await _questionService.GetQuestionByTag(tag);
            return result is null ? BadRequest(result) : Ok(result);
        }

    }
}
