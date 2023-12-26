using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Domain;

namespace VNH.Application.DTOs.Catalog.MultipleChoiceDto
{
    public class MultipleChoiceResponseDto
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int WorkTime { get; set; }

        public int NumberQuiz { get; set; } = 0;


        public UserShortDto?  UserShort { get; set; } = new UserShortDto();

        public List<QuizDto> Quizs { get; set; } = new List<QuizDto>();
    }
}
