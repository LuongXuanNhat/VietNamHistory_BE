﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("PostSubComment")]
    public partial class PostSubComment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? PreCommentId { get; set; }
        public Guid? UserId { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("PreCommentId")]
        [InverseProperty("PostSubComments")]
        public virtual PostComment PreComment { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostSubComments")]
        public virtual User User { get; set; }
    }
}