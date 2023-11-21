using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Domain;

namespace VNH.Application.DTOs.Catalog.Posts
{
    public class QuestionResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string SubId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
        public UserShortDto UserShort { get; set; } = new UserShortDto();
        public int ViewNumber { get; set; } = 0;
        public int CommentNumber { get; set; } = 0;
        public int LikeNumber { get; set; } = 0;
        public int SaveNumber { get; set; } = 0;
    }
}