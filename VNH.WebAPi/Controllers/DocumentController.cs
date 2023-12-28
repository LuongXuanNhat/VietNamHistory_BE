using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Document;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Documents;

namespace VNH.WebAPi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _documentService.GetAll();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateDocument([FromForm] CreateDocumentDto requestDto)
        {
            var result = await _documentService.Update(requestDto, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }


        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDto requestDto)
        {
            var result = await _documentService.Create(requestDto, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _documentService.Detail(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _documentService.Delete(Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("Save")]
        [Authorize]
        public async Task<IActionResult> GetSaveDocs([FromQuery] DocumentFpkDto docsFpk)
        {
            var result = await _documentService.GetSave(docsFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] DocumentFpkDto docsFpk)
        {
            var result = await _documentService.AddOrRemoveSaveDocs(docsFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyWord)
        {
            var result = await _documentService.Search(keyWord);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("MySave")]
        public async Task<IActionResult> GetMySave()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _documentService.GetMySave(userId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("MyDocument")]
        public async Task<IActionResult> GetMyDocument()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _documentService.GetMyDocument(userId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("SaveDownloads")]
        public async Task<IActionResult> SaveDownloads(Guid documentId)
        {
            await _documentService.SaveDownloads(documentId);
            return Ok();
        }
    }
}
