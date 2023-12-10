namespace VNH.Application.DTOs.Catalog.Forum.Question
{
    public class ReportQuestionDto
    {
        public string QuestionId { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid ReportId { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public bool Checked { get; set; } = false;
    }
}
