using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.HashTags
{
    public class TagDto
    {
        public Guid Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
    }
}
