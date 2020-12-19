using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        //In Instructor model, Column attribute was used to change coulmn name mapping.
        //Note: In the department here, the Column attribure is used to change SQL data type mapping. (The SQL server money type in the database.)
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /*
            The FK and navigation properties reflect the following relationships:
                - A department may or may not have an administrator.
                - An administrator is always an instructor. Therefore the InstructorID property is included as the FK to the Instructor entity.
         */
        public int? InstructorID { get; set; }

        public Instructor Administrator { get; set; }
        //A department may have many courses.
        public ICollection<Course> Courses { get; set; }
    }
}
