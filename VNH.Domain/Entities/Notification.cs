using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{
    [Table("Notification")]
    public class Notification
    {
        public Notification()
        {
            NotificationDetails = new HashSet<NotificationDetail>();
        }
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }

        [InverseProperty("Notification")]
        public virtual ICollection<NotificationDetail> NotificationDetails { get; set; }
    }
}
