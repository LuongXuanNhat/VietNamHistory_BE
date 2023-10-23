namespace VNH.Application.DTOs.Catalog.Reports
{
    public class ReportDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
