using System.ComponentModel.DataAnnotations;


namespace VNH.Domain.Entities
{
    public class PostTag
    {
        [Key]
        public int Id { get; set; }
        public string PostId { get; set; }
        public Guid TagId { get; set; }
    }
}
