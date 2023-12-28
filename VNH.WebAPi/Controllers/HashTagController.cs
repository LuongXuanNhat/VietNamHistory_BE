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
            var result = await _hasgTagService.GetAllTag(0);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
        [HttpGet("TopTag")]
        public async Task<IActionResult> GetTopTags(int numberTag)
        {
            var result = await _hasgTagService.GetAllTag(numberTag);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
    }
}
