using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstMidName { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Hire Date")]
        //Note: Multiple attributes can be on one line.
        [DataType(DataType.Date), Display(Name = "Hire Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        //An instructor can teach any number of courses, so CourseAssignment is defined as a collection
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        //An instructore can have at most one office, so the officeAssignment property holds a single officeAssignment entity.
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}