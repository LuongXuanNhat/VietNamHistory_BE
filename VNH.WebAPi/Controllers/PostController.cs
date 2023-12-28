using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Posts;

namespace VNH.WebAPi.Controllers
{
    [Route("Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("Discover")]
        public async Task<IActionResult> Index()
        {
            var result = await _postService.GetAll();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("DiscoverMobile")]
        public async Task<IActionResult> IndexMobile()
        {
            var result = await _postService.GetAllMobile();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("RandomArticle")]
        public async Task<IActionResult> RandomPost(int quantity = 0)
        {
            var result = await _postService.GetRandomPost(quantity);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Create(requestDto, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Update(requestDto, User.Identity.Name);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var result = await _postService.Detail(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _postService.Delete(Id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("Remove")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAdmin(string Id)
        {
            var result = await _postService.DeleteAdmin(Id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Like")]
        public async Task<IActionResult> GetLikePost([FromQuery] PostFpkDto postFpk)
        {
            var result = await _postService.GetLike(postFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        
        [HttpPost("Like")]
        [Authorize]
        public async Task<IActionResult> Like([FromForm] PostFpkDto postFpk)
        {
            var result = await _postService.AddOrUnLikePost(postFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Save")]
        [Authorize]
        public async Task<IActionResult> GetSavePost([FromQuery] PostFpkDto postFpk)
        {
            var result = await _postService.GetSave(postFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("MyPostSaved")]
        [Authorize]
        public async Task<IActionResult> GetMyPostSaved()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _postService.GetMyPostSaved(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("MyPost")]
        [Authorize]
        public async Task<IActionResult> GetMyPost()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _postService.GetMyPost(id);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] PostFpkDto postFpk)
        {
            var result = await _postService.AddOrRemoveSavePost(postFpk);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Report")]
        [Authorize]
        public async Task<IActionResult> Report([FromBody] ReportPostDto reportPostDto)
        {
            var result = await _postService.ReportPost(reportPostDto);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpGet("Report")]
        [Authorize(Roles = "admin, sensor")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _postService.GetReport();
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("FindByTag")]
        public async Task<IActionResult> GetPostByTag(string tag)
        {
            var result = await _postService.GetPostByTag(tag);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string keyWord)
        {
            var result = await _postService.SearchPosts(keyWord);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Chat")]
        public async Task<IActionResult> GetComments(string PostId)
        {
            var result = await _postService.GetComment(PostId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpPost("Chat")]
        [Authorize]
        public async Task<IActionResult> CreateComment(CommentPostDto comment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _postService.CreateComment(comment, userId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
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
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
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
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Chat/NumberComment")]
        public async Task<IActionResult> GetCommentPost(string PostId)
        {
            var result = await _postService.GetComment(PostId);
            var numberResult = new ApiSuccessResult<int>(result.ResultObj.Count);
            return numberResult is null ? BadRequest(numberResult) : Ok(numberResult);
        }

        [HttpGet("FindByTopic")]
        public async Task<IActionResult> FindByTopic(string TopicName)
        {
            var result = await _postService.FindByTopic(TopicName);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
    }
}
