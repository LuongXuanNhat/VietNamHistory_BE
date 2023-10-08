using Microsoft.AspNetCore.Http;

namespace VNH.Application.DTOs.Catalog.Posts
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public Guid TopicId { get; set; }
        public List<string> Tag { get; set; }
    }
}
 