using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Users;

namespace VNH.Application.DTOs.Catalog.Document
{
    public class DocumentReponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string FileName {  get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserShortDto  UserShort { get; set; } = new UserShortDto();
        public int ViewNumber { get; set; } = 0;
        public int DownloadNumber { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
    }
}
