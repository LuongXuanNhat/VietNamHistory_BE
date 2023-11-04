using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.Services.Catalog.Users;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserDetail()
        {
            var result = await _userService.GetUserDetail(User.Identity.Name);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto request)
        {
            var result = await _userService.Update(request);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Image")]
        [Authorize]
        public async Task<IActionResult> GetImage()
        {
            var result = await _userService.GetImage(User.Identity.Name);
            return result.IsSuccessed ? Ok(result.ResultObj) : BadRequest(result.Message);
        }

        [HttpPost("Image")]
        [Authorize]
        public async Task<IActionResult> SetImage(IFormFile image)
        {
            var result = await _userService.SetImageUser(User.Identity.Name, image);
            return result.IsSuccessed ? Ok(result.ResultObj) : BadRequest(result.Message);
        }
    }
}
