using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.Forum.Answer
{
    public class CreateAnswerDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? AuthorId { get; set; }
        public string QuestionId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
       
    }

    public class SubAnswerDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PreAnswerId { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid AuthorId { get; set; }


    }
}
