﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("Exercise")]
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseDetails = new HashSet<ExerciseDetail>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Title { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Image { get; set; }
        public TimeSpan? Time { get; set; }
        public Guid? QuizId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("Id")]
        [InverseProperty("Exercise")]
        public virtual Lesson IdNavigation { get; set; }
        [InverseProperty("IdNavigation")]
        public virtual Quiz Quiz { get; set; }
        [InverseProperty("Exercise")]
        public virtual ICollection<ExerciseDetail> ExerciseDetails { get; set; }
    }
}