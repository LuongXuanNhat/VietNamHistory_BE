using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.Posts
{
    public class CommentPostDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        public string PostId { get; set; } = String.Empty;
        public UserShortDto? UserShort { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public List<SubCommentDto>? SubComment { get; set; }
    }
    public class SubCommentDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        public UserShortDto? UserShort { get; set; }
    }
}
