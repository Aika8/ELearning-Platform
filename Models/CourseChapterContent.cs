using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningMVC.Models
{
    public class CourseChapterContent
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CourseChapterId { get; set; }
        [ForeignKey("CourseChapterId")]
        public virtual CourseChapter CourseChapter { get; set; }

        [Required]
        [Display(Name = "Content Title")]
        public String ContentName { get; set; }

        [Required]
        public String File { get; set; }


    }
}
