using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.Interfaces.Posts;
using VNH.Domain;

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
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _postService.Delete(Id);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Like")]
        [Authorize]
        public async Task<IActionResult> Like([FromForm] string PostId, [FromForm] string UserId)
        {
            var result = await _postService.AddOrUnLikePost(PostId, UserId);
            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] string PostId, [FromForm] string UserId)
        {
            var result = await _postService.AddOrRemoveSavePost(PostId, UserId);
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
