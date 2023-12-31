﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.Domain
{
    [Table("Lesson")]
    public partial class Lesson
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(500)]
        public string Title { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string UrlVideo { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? ExerciseId { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("CourseId")]
        [InverseProperty("Lessons")]
        public virtual Course Course { get; set; }
        [InverseProperty("IdNavigation")]
        public virtual Exercise Exercise { get; set; }

    }
}