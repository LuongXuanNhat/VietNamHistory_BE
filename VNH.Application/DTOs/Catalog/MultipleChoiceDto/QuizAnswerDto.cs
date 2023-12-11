using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.MultipleChoiceDto
{
    public class QuizAnswerDto
    {

        public Guid Id { get; set; } = Guid.Empty;

        public string Content { get; set; } = string.Empty;

        public bool isCorrect { get; set; } = false;


    }
}
