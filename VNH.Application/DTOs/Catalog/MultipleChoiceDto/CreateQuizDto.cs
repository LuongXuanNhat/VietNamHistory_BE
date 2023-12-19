using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.MultipleChoiceDto
{
    public class CreateQuizDto
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
         
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IFormFile? File { get; set; }

        public int WorkTime { get; set; } 

    }
}
