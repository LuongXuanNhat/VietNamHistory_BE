﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("PostDetail")]
    public partial class PostDetail
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(255)]
        public string PostId { get; set; }
        public Guid? UserId { get; set; }
        public int? ViewNumber { get; set; }
        public int? CommentNumber { get; set; }
        public int? LikeNumber { get; set; }
        public int? SaveNumber { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostDetails")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PostDetails")]
        public virtual User User { get; set; }
    }
}