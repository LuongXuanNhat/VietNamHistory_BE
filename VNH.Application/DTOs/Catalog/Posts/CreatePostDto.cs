using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VNH.Application.DTOs.Catalog.Posts
{
    public class CreatePostDto
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public Guid TopicId { get; set; }
        public List<string>? Tag { get; set; } = new List<string>();
    }
}
