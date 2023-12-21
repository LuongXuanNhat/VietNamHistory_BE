using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.Interfaces.Common;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleImageController : ControllerBase
    {
        private readonly IImageService _image;
        public ArticleImageController(IImageService imageService) { 
            _image = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    if (file.Length > 3 * 1024 * 1024) // 3 MB
                    {
                        return BadRequest("Kích thước file không được lớn hơn 3 MB");
                    }
                    var fileName = file.FileName;
                    var rerult = await _image.SaveFile(file);
                    return Ok(rerult);
                } else { return BadRequest(); }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
