using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{

    [Table("ExamHistory")]
    public partial class ExamHistory
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid MultipleChoiceId { get; set; }

        public Guid UserId { get; set; }

        public int Scores { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CompletionTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StarDate { get; set; }



        [ForeignKey("MultipleChoiceId")]
        public virtual MultipleChoice MultipleChoice { get; set; }







    }
}
