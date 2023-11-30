using System.ComponentModel.DataAnnotations.Schema;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.Forum.Answer
{
    public class AnswerQuestionDto
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? AuthorId { get; set; }

        public string QuestionId { get; set; } = string.Empty;
        public UserShortDto? UserShort { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool Confirm { get; set; } = false;
        public bool MostConfirm { get; set; } = false;
        public List<SubAnswerQuestionDto>? SubAnswer { get; set; } = new();

    }

    public class SubAnswerQuestionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PreAnswerId { get; set; }
        public Guid AuthorId { get; set; }  
        public string Content { get; set; } = string.Empty;
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public UserShortDto? UserShort { get; set; }


    }

}
