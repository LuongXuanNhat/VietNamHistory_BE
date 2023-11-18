using Microsoft.AspNetCore.Http;
namespace VNH.Application.DTOs.Catalog.Document
{
    public class CreateDocumentDto
    {

        public Guid? Id { get; set; } = Guid.NewGuid();
        public string? SubId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? FileName { get; set; }

    }
}
