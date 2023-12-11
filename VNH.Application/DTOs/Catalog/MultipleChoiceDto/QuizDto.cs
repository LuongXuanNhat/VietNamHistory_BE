using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.MultipleChoiceDto
{
    public class QuizDto
    {

        public Guid Id { get; set; } = Guid.Empty; 


        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        public List<QuizAnswerDto>? QuizAnswers { get; set; } = new List<QuizAnswerDto>();
    }
}
