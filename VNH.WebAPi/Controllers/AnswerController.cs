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
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateAnswer(AnswerQuestionDto answer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _answerService.CreateAnswer(answer, id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
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
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer(string idAnswer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(id))
            {
                return BadRequest();
            }
            var result = await _answerService.DeteleAnswer(idAnswer);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpPost("SubAnswer")]
        [Authorize]
        public async Task<IActionResult> CreateSubAnswer(SubAnswerQuestionDto subAnswer)
        {
            var result = await _answerService.CreateSubAnswer(subAnswer);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
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
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpDelete("DeleteSub")]
        [Authorize]
        public async Task<IActionResult> DeleteSubAnswer(string idSubAnswer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(id))
            {
                return BadRequest();
            }
            var result = await _answerService.DeteleSubAnswer(idSubAnswer);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmAnswer(AnswerFpkDto answer)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(id == answer.QuestionUserId)
            {
                var result = await _answerService.ConfirmedByQuestioner(answer.AnswerId);
                return Ok(result);
            } 
            return Ok();
 
        }
        [HttpPost("Vote")]
        [Authorize]
        public async Task<IActionResult> Vote(AnswerFpkDto answer)
        {
            var result = await _answerService.VoteConfirmByUser(answer);
            return Ok(result);
        }

        [HttpGet("GetMyVote")]
        [Authorize]
        public async Task<IActionResult> GetVote(string answerId, string userId)
        {
            var result = await _answerService.GetMyVote(answerId, userId);
            return Ok(result);
        }
    }
}
