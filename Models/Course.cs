using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningMVC.Models
{
    public class Course
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string CourseTitle { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string CourseBrief { get; set; }

        [Required]
        [StringLength(30)]
        public string Teacher { get; set; }

        [Required]
        [StringLength(80)]
        public string Type { get; set; }

        [Required]
        public int? LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string ImgUrl { get; set; }


        public virtual ICollection<CourseChapter> CourseChapter { get; set; }


        public List<Student> Students { get; set; } = new List<Student>();
    }
}
