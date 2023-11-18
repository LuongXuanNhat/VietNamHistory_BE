using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.Chats;
using VNH.Application.Interfaces.Posts;

namespace VNH.WebAPi.Controllers
{
    [Route("Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("Discover")]
        public async Task<IActionResult> Index()
        {
            var result = await _postService.GetAll();
            return result is null ? BadRequest(result) : Ok(result);
        }


        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Create(requestDto, User.Identity.Name);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Update(requestDto, User.Identity.Name);
            return result == null ? BadRequest(result) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _postService.Detail(id);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _postService.Delete(Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAdmin(string Id)
        {
            var result = await _postService.DeleteAdmin(Id);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Like")]
        public async Task<IActionResult> GetLikePost([FromQuery] PostFpkDto postFpk)
        {
            var result = await _postService.GetLike(postFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Like")]
        [Authorize]
        public async Task<IActionResult> Like([FromForm] PostFpkDto postFpk)
        {
            var result = await _postService.AddOrUnLikePost(postFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Save")]
        [Authorize]
        public async Task<IActionResult> GetSavePost([FromQuery] PostFpkDto postFpk)
        {
            var result = await _postService.GetSave(postFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] PostFpkDto postFpk)
        {
            var result = await _postService.AddOrRemoveSavePost(postFpk);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Report")]
        [Authorize]
        public async Task<IActionResult> Report([FromBody] ReportPostDto reportPostDto)
        {
            var result = await _postService.ReportPost(reportPostDto);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpGet("Report")]
        [Authorize(Roles = "admin, sensor")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _postService.GetReport();
            return result is null ? BadRequest(result) : Ok(result);
        }
      

    }
}
