using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.Chats;
using VNH.Application.Interfaces.Posts;
using VNH.Domain;

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
        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetMyPostSaved()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _postService.GetMyPostSaved(id);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpGet("MyPost")]
        [Authorize]
        public async Task<IActionResult> GetMyPost()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _postService.GetMyPost(id);
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
        [HttpGet("FindByTag")]
        public async Task<IActionResult> GetPostByTag(string tag)
        {
            var result = await _postService.GetPostByTag(tag);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Chat")]
        public async Task<IActionResult> GetComments(string PostId)
        {
            var result = await _postService.GetComment(PostId);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Chat")]
        [Authorize]
        public async Task<IActionResult> CreateComment(CommentPostDto comment)
        {
            var result = await _postService.CreateComment(comment);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpPut("Chat")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(CommentPostDto comment)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(comment.UserId.ToString()))
            {
                return BadRequest();
            }
            var result = await _postService.UpdateComment(comment);
            return result is null ? BadRequest(result) : Ok(result);
        }
        [HttpDelete("Chat")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string idComment)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null || !id.Equals(id))
            {
                return BadRequest();
            }
            var result = await _postService.DeteleComment(idComment);
            return result is null ? BadRequest(result) : Ok(result);
        }
    }
}
