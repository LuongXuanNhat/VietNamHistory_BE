using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.MultipleChoiceDto
{
    public class QuizRequestDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public List<QuizAnswerRequestDto> QuizAnswers { get; set; } = new List<QuizAnswerRequestDto>();
    }

    public class QuizAnswerRequestDto

    {
        public Guid Id { get; set; }


        public string Content { get; set; } = string.Empty;


        public bool isCorrect { get; set; } = false;


    }
}
