



using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.ExamHistory
{
    public class ExamHistoryResponseDto
    {
        public Guid? Id { get; set; }

        public MultipleChoiceResponseDto? multipleChoiceResponseDto { get; set; } = new MultipleChoiceResponseDto();

        public UserShortDto? UserShortDto { get; set; } = new UserShortDto();

        public int numberQuiz { get; set; } = 0;

        public float Scores { get; set; }

        public int CompletionTime { get; set; }

        public DateTime StarDate { get; set; }

    }
}
