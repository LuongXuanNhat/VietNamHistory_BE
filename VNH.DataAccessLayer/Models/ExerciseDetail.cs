﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("ExerciseDetail")]
    public partial class ExerciseDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ExerciseId { get; set; }
        public double? TestMark { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TestTime { get; set; }

        [ForeignKey("ExerciseId")]
        [InverseProperty("ExerciseDetails")]
        public virtual Exercise Exercise { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ExerciseDetails")]
        public virtual User User { get; set; }
    }
}