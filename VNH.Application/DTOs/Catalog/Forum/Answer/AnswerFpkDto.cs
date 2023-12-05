using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.Forum.Answer
{
    public class AnswerFpkDto
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string QuestionUserId { get; set; } = string.Empty;

    }
}
