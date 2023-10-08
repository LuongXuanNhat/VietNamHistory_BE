using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.Users
{
    public class UserShortDto 
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Image { get; set; } = string.Empty;
    }
}
