namespace VNH.Application.DTOs.Catalog.Posts
{
    public class CommentPostDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PostId { get; set; } = String.Empty;
        public Guid UserId { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
