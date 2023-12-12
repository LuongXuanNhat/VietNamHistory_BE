using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.ExamHistory;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.ExamHistory;
using VNH.Domain;

namespace VNH.WebAPi.Controllers
{

    [Route("ExamHistory")]
    [ApiController]
    public class ExamHistoryController : ControllerBase
    {
        private readonly IExamHistoryService _examHistoryService;

        public ExamHistoryController(IExamHistoryService examHistoryService)
        {
            _examHistoryService = examHistoryService;
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateExamHistoryDto requestDto)
        {
            var result = await _examHistoryService.Create(requestDto,User.Identity.Name );
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] CreateExamHistoryDto requestDto)
        {
            var result = await _examHistoryService.Update(requestDto, User.Identity.Name);
            return result == null ? BadRequest(result) : Ok(result);
        }


        [HttpGet("GetMyExamHistory")]
        public async Task<IActionResult> GetMyExamHistory(string id)
        {
            var result = await _examHistoryService.GetMyExamHistory(id);

            return result is null ? BadRequest(result) : Ok(result);


        }

    }
}
