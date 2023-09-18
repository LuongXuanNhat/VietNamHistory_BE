﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.Domain
{
    [Table("QuestionSave")]
    public partial class QuestionSave
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        [InverseProperty("QuestionSaves")]
        public virtual Question Question { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("QuestionSaves")]
        public virtual User User { get; set; }
    }
}