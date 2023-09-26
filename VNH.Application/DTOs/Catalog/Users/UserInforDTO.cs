using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VNH.Domain.Enums;

namespace VNH.Application.DTOs.Catalog.Users
{
    public class UserInforDTO
    {
        [StringLength(100)]
        public string Fullname { get; set; }
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(50)]
        public Gender Gender { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
    }
}
