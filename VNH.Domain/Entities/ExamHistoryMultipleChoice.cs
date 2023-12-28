using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{
    public class ExamHistoryMultipleChoice
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ExamHistoryId { get; set; }
        public ExamHistory ExamHistory { get; set; }

        public Guid MultipleChoiceId { get; set; }
        public MultipleChoice MultipleChoice { get; set; }
    }
}
