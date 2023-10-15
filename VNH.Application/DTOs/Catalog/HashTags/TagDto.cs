using System.ComponentModel.DataAnnotations;

namespace VNH.Application.DTOs.Catalog.HashTags
{
    public class TagDto
    {
        public Guid Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
    }
}
