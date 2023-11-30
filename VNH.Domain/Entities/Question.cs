﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VNH.Domain
{
    [Table("Question")]
    public partial class Question
    {
        public Question()
        {
            QuestionLikes = new HashSet<QuestionLike>();
            QuestionReportDetails = new HashSet<QuestionReportDetail>();
            QuestionSaves = new HashSet<QuestionSave>();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(500)]
        public string Title { get; set; }
        public string SubId { get; set; }
        public string Content { get; set; }
        public int ViewNumber { get; set; }
        public Guid? AuthorId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
       

        [ForeignKey("AuthorId")]
        [InverseProperty("Questions")]
        public virtual User Author { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionTag> QuestionTag { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionLike> QuestionLikes { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionReportDetail> QuestionReportDetails { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionSave> QuestionSaves { get; set; }
        [InverseProperty("Questions")]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}