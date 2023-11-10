using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VNH.Domain.Enums;

namespace VNH.Application.DTOs.Catalog.Users
{
    public class UserUpdateDto
    {
        [StringLength(100)]
        public string? Fullname { get; set; } = string.Empty;
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public virtual string? Email { get; set; } = string.Empty;
        public virtual string? PhoneNumber { get; set; } = string.Empty;
        public virtual string? Introduction { get; set; } = string.Empty;
    }
}
