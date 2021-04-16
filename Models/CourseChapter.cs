using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningMVC.Models
{
    public class CourseChapter
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [Required]
        [Display(Name = "Chapter Title")]
        public String ChapterTitle { get; set; }

        [Required]
        [Display(Name = "Time Required")]
        public int TimeRequiredInSec { get; set; }

        [Display(Name = "Course Chapter Content")]
        public virtual ICollection<CourseChapterContent> CourseChapterContent { get; set; }
    }
}
