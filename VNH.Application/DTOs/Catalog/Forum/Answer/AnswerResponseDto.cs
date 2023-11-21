using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.Forum.Answer
{
    public class AnswerResponseDto
    {

        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public string QuestionId { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime? PubDate { get; set; }
        public UserShortDto UserShort { get; set; } = new UserShortDto();
        public bool Confirm { get; set; }
        public bool MostConfirm { get; set; }
        public List<SubAnswerResponseDto>? SubAnserwer { get; set; }

    }

    public class SubAnswerResponseDto
    {
        public Guid Id { get; set; }
        public Guid PreAnswerId { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column(TypeName = "datetime")]
        public DateTime? PubDate { get; set; }

        public UserShortDto? UserShort { get; set; }

    }

}
