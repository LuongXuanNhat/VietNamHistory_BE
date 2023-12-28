using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.Interfaces.Catalog.MultipleChoices;

namespace VNH.WebAPi.Controllers
{
    [Route("MultipleChoice")]
    [ApiController]
    public class MultiChoiceController : ControllerBase
    {

        public readonly IMultipleChoiceService _multipleChoiceService;

        public MultiChoiceController(IMultipleChoiceService multipleChoiceService)
        {

            _multipleChoiceService = multipleChoiceService;
        }


        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateMultiChoice([FromForm] CreateQuizDto requestDto)
        {
            var result = await _multipleChoiceService.Create(requestDto, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _multipleChoiceService.Detail(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _multipleChoiceService.GetAll();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] CreateQuizDto requestDto)
        {
            var result = await _multipleChoiceService.Update(requestDto, User.Identity.Name);
            return result == null ? BadRequest(result) : Ok(result);
        }


        [HttpPut("QuizById")]
        [Authorize]
        public async Task<IActionResult> UpdateQuizById([FromBody] QuizDto requestDto)
        {
            var result = await _multipleChoiceService.UpdateQuizById(requestDto);
            return result == null ? BadRequest(result) : Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string idMultipleChoice)
        {

            var result = await _multipleChoiceService.Delete(idMultipleChoice, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);

        }

        [HttpDelete("DeleteQuizById")]
        [Authorize]
        public async Task<IActionResult> DeleteQuizById(string idQuiz)
        {

            var result = await _multipleChoiceService.DeleteQuizById(idQuiz);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);

        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string keyWord)
        {
            var result = await _multipleChoiceService.Search(keyWord);

            return !result.IsSuccessed ? BadRequest(result) : Ok(result);


        }


        [HttpGet("MyMultipleChoice")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetMyPost()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _multipleChoiceService.GetMyMultipleChoice(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }



    }

}
