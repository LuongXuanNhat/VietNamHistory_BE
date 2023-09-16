﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.DataAccessLayer.Models
{
    [Table("Quiz")]
    public partial class Quiz
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        [StringLength(500)]
        public string Answer1 { get; set; }
        [StringLength(500)]
        public string Answer2 { get; set; }
        [StringLength(500)]
        public string Answer3 { get; set; }
        [StringLength(500)]
        public string Answer4 { get; set; }
        [StringLength(500)]
        public string RightAnswer { get; set; }

        [ForeignKey("Id")]
        [InverseProperty("Quiz")]
        public virtual Exercise IdNavigation { get; set; }
    }
}