﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("CourseRating")]
    public partial class CourseRating
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? UserId { get; set; }
        public int? Score { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("CourseRatings")]
        public virtual Course Course { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CourseRatings")]
        public virtual User User { get; set; }
    }
}