using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Entities
{

    [Table("ListQuiz")]
    public partial class MultipleChoice
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }
        [StringLength(500)]

        public string Description { get; set; }
        [StringLength(500)]

        public string Author { get; set; }
        [StringLength(500)]



    }
}
