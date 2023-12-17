using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.Interfaces.Catalog.Roles;
using VNH.Application.Interfaces.Common;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

      /*  [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }*/
    }
}
