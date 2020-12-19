﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        // Note: The [Key] attribute is used to identify a property as the primary key (PK) when the property name is something other than classnameID or ID.
        // The OfficeAssignment PK is also its foreign key (FK) to the Instructor entity.
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public Instructor Instructor { get; set; }
    }
}
