using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Document;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Documents;

namespace VNH.WebAPi.Controllers
{

    [Route("Document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }


        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var result = await _documentService.GetAll();
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateDocument([FromForm] CreateDocumentDto requestDto)
        {
            var result = await _documentService.Update(requestDto, User.Identity.Name);
            return result == null ? BadRequest(result) : Ok(result);
        }


        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDto requestDto)
        {
            var result = await _documentService.Create(requestDto, User.Identity.Name);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _documentService.Detail(id);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _documentService.Delete(Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return result is null ? BadRequest(result) : Ok(result);
        }



    }
}
