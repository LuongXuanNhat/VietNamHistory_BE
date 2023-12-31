﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VNH.Domain
{
    [Table("PostComment")]
    public partial class PostComment
    {
        public PostComment()
        {
            PostSubComments = new HashSet<PostSubComment>();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(255)]
        public string PostId { get; set; }
        public Guid? UserId { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostComments")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostComments")]
        public virtual User User { get; set; }
        [InverseProperty("PreComment")]
        public virtual ICollection<PostSubComment> PostSubComments { get; set; }
    }
}