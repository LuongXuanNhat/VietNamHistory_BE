using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public IActionResult Home()
        {
            return Ok("Đây là trang chủ Người Kể Sử");
        }
    }
}
