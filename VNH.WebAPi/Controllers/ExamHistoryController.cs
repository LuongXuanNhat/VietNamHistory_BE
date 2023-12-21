using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.ExamHistory;
using VNH.Application.Interfaces.Catalog.ExamHistory;

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
        public async Task<IActionResult> Create([FromForm] CreateExamHistoryDto requestDto)
        {
            var result = await _examHistoryService.Create(requestDto,User.Identity.Name );
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("GetExamHistory")]
        public async Task<IActionResult> GetExamHistory(string id)
        {
            var result = await _examHistoryService.GetExamHistory(id);

            return !result.IsSuccessed ? BadRequest(result) : Ok(result);


        }


        [HttpGet("GetMyExamHistory")]
        public async Task<IActionResult> GetMyExamHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _examHistoryService.GetMyExamHistory(userId);

            return !result.IsSuccessed ? BadRequest(result) : Ok(result);


        }

    }
}
