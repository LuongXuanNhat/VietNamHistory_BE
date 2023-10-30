using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VNH.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace VNH.Application.DTOs.Catalog.Users
{
    public class UserDetailDto
    {
        [StringLength(200)]
        public string Fullname { get; set; } = string.Empty;
        public virtual string UserName { get; set; } = string.Empty;
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(50)]
        public Gender Gender { get; set; }
        public string? Image { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string PhoneNumber { get; set; } = string.Empty;
        public virtual string Introduction { get; set; } = string.Empty;
    }
}
