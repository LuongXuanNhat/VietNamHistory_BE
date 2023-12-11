using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{

    [Table("QuizAnswer")]
    public partial class QuizAnswer
    {

        [Key]
        public Guid Id { get; set; }

        public Guid QuizId { get; set; }


        public string Content { get; set; }
        [StringLength(500)]


        public bool isCorrect { get; set; } = false;

        [ForeignKey("QuizId")]
       
        public virtual Quiz Quiz { get; set; }



    }
}
