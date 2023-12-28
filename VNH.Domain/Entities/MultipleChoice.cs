using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{

    [Table("MultipleChoise")]
    public partial class MultipleChoice
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public int WorkTime { get; set; }

        public Guid UserId { get; set; }

        [InverseProperty("MultipleChoice")]
        public virtual ICollection<Quiz> Quiz { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [InverseProperty("MultipleChoice")]
        public virtual ICollection<ExamHistory> ExamHistory { get; set; }
    }
}
