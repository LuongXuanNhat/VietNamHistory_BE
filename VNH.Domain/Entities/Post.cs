﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VNH.Domain.Entities;

namespace VNH.Domain
{
    [Table("Post")]
    public partial class Post
    {
        public Post()
        {
            PostComments = new HashSet<PostComment>();
            PostLikes = new HashSet<PostLike>();
            PostReportDetails = new HashSet<PostReportDetail>();
            PostSaves = new HashSet<PostSave>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [StringLength(500)]
        public string SubId { get; set; } = string.Empty;
        [StringLength(255)]
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid TopicId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Image { get; set; } = string.Empty;
        public int ViewNumber { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("TopicId")]
        [InverseProperty("Posts")]
        public virtual Topic Topic { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Posts")]
        public virtual User User { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostComment> PostComments { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostLike> PostLikes { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostReportDetail> PostReportDetails { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostTag> PostTags { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostSave> PostSaves { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<TopicDetail> TopicDetails { get; set; }
    }
}