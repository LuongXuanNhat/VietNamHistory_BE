using Microsoft.AspNetCore.Http;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Domain;

namespace VNH.Application.DTOs.Catalog.Posts
{
    public class PostResponsetDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Topic { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
        public UserShortDto User { get; set; }
        public int ViewNumber { get; set; } = 0;
        public int CommentNumber { get; set; } = 0;
        public int LikeNumber { get; set; } = 0;
        public int SaveNumber { get; set; } = 0;
    }
}