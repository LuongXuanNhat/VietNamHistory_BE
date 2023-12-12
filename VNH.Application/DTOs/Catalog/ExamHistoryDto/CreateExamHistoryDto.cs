using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.ExamHistory
{
    public class CreateExamHistoryDto
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MultipleChoiceId { get; set; }
        public Guid UserId { get; set; }

        public int Scores { get; set; }

        public int CompletionTime { get; set; }

        public DateTime StarDate { get; set; } = DateTime.Now;

    }
}
