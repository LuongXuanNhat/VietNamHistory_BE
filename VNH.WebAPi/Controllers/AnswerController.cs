using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.Forum;

namespace VNH.WebAPi.Controllers
{


    [Route("Answer")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService; 

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAnswers(string questionId)
        {
            var result = await _answerService.GetAnswer(questionId);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateAnswer(AnswerQuestionDto answer)
        {
            var result = await _answerService.CreateAnswer(answer);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAnswer(AnswerQuestionDto answer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(answer.AuthorId.ToString()))
            {
                return BadRequest();
            }
            var result = await _answerService.UpdateAnswer(answer);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer(string idAnswer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(id))
            {
                return BadRequest();
            }
            var result = await _answerService.DeteleAnswer(idAnswer);
            return result is null ? BadRequest(result) : Ok(result);
        }


        [HttpPost("SubAnswer")]
        [Authorize]
        public async Task<IActionResult> CreateSubAnswer(SubAnswerQuestionDto subAnswer)
        {
            var result = await _answerService.CreateSubAnswer(subAnswer);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut("SubAnswer")]
        [Authorize]
        public async Task<IActionResult> UpdateSubAnswer(SubAnswerQuestionDto subAnswer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(subAnswer.AuthorId.ToString()))
            {
                return BadRequest();
            }
            var result = await _answerService.UpdateSubAnswer(subAnswer);
            return result is null ? BadRequest(result) : Ok(result);
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteSubAnswer(string idSubAnswer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(id))
            {
                return BadRequest();
            }
            var result = await _answerService.DeteleSubAnswer(idSubAnswer);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Vote")]
        [Authorize]
        public async Task<IActionResult> Vote([FromForm] AnswerFpkDto answerFpk)
        {
            var result = await _answerService.ConfirmOrNoConfirm(answerFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }


    }
}
