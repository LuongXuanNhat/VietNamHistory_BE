using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Domain.Enums;

namespace VNH.Domain.Entities
{
    [Table("NotificationDetails")]
    public class NotificationDetail
    {
        public Guid Id { get; set; }
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public Guid? IdObject { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public string? Url { get; set; }
        public Confirm IsRead { get; set; }


        // Relationship
        [ForeignKey("NotificationId")]
        [InverseProperty("NotificationDetails")]
        public virtual Notification? Notification { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("NotificationDetails")]
        public virtual User? User { get; set; }
    }
}
