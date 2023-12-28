using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.DTOs.Catalog.Notifications
{
    public class NotificationDto
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? NotificationId { get; set; } = Guid.Empty;
        public Guid? IdObject { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; }
        public string? Url { get; set; }

    }
}
