﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.Domain
{
    [Table("PostReport")]
    public partial class PostReport
    {
        public PostReport()
        {
            PostReportDetails = new HashSet<PostReportDetail>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty("Report")]
        public virtual ICollection<PostReportDetail> PostReportDetails { get; set; }
    }
}