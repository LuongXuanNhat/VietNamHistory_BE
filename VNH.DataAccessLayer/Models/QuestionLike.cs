﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("QuestionLike")]
    public partial class QuestionLike
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? UserId { get; set; }

        [ForeignKey("QuestionId")]
        [InverseProperty("QuestionLikes")]
        public virtual Question Question { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("QuestionLikes")]
        public virtual User User { get; set; }
    }
}