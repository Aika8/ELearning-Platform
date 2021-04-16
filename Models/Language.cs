using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningMVC.Models
{
    public class Language : IValidatableObject
    {



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public String Code { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errormsg = new List<ValidationResult>();
            if (Code == null)
            {
                errormsg.Add(new ValidationResult("this field cannot be empty", new[] { "Code" }));
                return errormsg;
            }

            if (Code.Length > 3)
            {
                errormsg.Add(new ValidationResult("CODE cannot have length grater than 3", new[] { "Code" }));
            }
            return errormsg;
        }
    }
}
