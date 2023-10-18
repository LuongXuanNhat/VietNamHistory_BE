using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VNH.Domain.Entities
{
    public class PostTag
    {
        [Key]
        public int Id { get; set; }
        public string PostId { get; set; } = string.Empty;
        public Guid TagId { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostTags")]
        public virtual Post? Post { get; set; }

        [ForeignKey("TagId")]
        [InverseProperty("PostTags")]
        public virtual Tag? Tag { get; set; }

    }
}
