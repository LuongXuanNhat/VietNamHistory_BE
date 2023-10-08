using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.Interfaces.Catalog.HashTags;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HashTagController : ControllerBase
    {
        private readonly IHashTag _hasgTagService;

        public HashTagController(IHashTag hashTagService) {
            _hasgTagService = hashTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _hasgTagService.GetAll());
        }

        
    }
}
