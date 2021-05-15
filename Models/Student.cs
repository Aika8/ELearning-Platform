using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELearningMVC.Models
{
    public class Student : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();


    }
}
