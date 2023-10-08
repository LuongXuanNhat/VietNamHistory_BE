using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.Posts;
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

        [HttpGet("All")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }

        
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Create(requestDto, User.Identity.Name);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string Id)
        {
            var result = await _postService.Detail(Id);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
