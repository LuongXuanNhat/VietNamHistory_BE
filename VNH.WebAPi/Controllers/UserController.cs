using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.Users;
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserById(id);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }



        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto request)
        {
            var result = await _userService.Update(request);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateForAdmin(Guid id, [FromForm] UserUpdateDto request)
        {
            var result = await _userService.UpdateForAdmin(id,request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Image")]
        [Authorize]
        public async Task<IActionResult> GetImage()
        {
            var result = await _userService.GetImage(User.Identity.Name);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Image")]
        [Authorize]
        public async Task<IActionResult> SetImage(IFormFile image)
        {
            var result = await _userService.SetImageUser(User.Identity.Name, image);
            return result.IsSuccessed ? Ok(result.ResultObj) : BadRequest(result.Message);
        }

        [HttpGet("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.getAllUser();
            return result.IsSuccessed ? Ok(result.ResultObj) : BadRequest(result);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RoleAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
