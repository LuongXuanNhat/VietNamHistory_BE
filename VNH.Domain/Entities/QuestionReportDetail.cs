﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.Domain
{
    [Table("QuestionReportDetail")]
    public partial class QuestionReportDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? QuestionReportId { get; set; }
        public Guid? UserId { get; set; }
        [StringLength(255)]
        public string Description { get; set; }

        [ForeignKey("QuestionId")]
        [InverseProperty("QuestionReportDetails")]
        public virtual Question Question { get; set; }
        [ForeignKey("QuestionReportId")]
        [InverseProperty("QuestionReportDetails")]
        public virtual QuestionReport QuestionReport { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("QuestionReportDetails")]
        public virtual User User { get; set; }
    }
}