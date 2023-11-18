
namespace VNH.Application.DTOs.Catalog.Posts
{
    public class ReportPostDto
    {
        public string PostId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReportId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public bool Checked { get; set; } = false;
    }
}
