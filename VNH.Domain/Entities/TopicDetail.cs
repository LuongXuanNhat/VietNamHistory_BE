﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VNH.Domain
{
    [Table("TopicDetail")]
    public partial class TopicDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? TopicId { get; set; }
        public string PostId { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("TopicDetails")]
        public virtual Post Post { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("TopicDetails")]
        public virtual Topic Topic { get; set; }
    }
}