using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.Forum.Question
{
    public class CreateQuestionDto
    {
        public Guid? Id { get; set; } = Guid.NewGuid();

        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public List<string>? Tag { get; set; }

    }
}
