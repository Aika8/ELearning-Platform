using ELearningMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ELearningMVC.Models
{
    public class CourseTypeViewModel
    {
        public List<Course> Courses { get; set; }
        public SelectList Type { get; set; }
        public string CourseType { get; set; }
        public string SearchString { get; set; }
    }
}
