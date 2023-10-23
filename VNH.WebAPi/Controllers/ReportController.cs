using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.Reports;
using VNH.Application.Interfaces.Catalog.Reports;

namespace VNH.WebAPi.Controllers
{
    [Route("Report")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService) {
            _reportService = reportService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportService.GetById(id);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportService.GetAll();
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ReportDto report)
        {
            var result = await _reportService.Create(report);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ReportDto report)
        {
            var result = await _reportService.Update(report);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reportService.Delete(id);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

    }
}
