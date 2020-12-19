using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }
        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID)
        {
            InstructorData = new InstructorIndexData();
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                //Comment out because user wants to see this info in particular condition.
                //.Include(i => i.CourseAssignments)
                //    .ThenInclude(i => i.Course)
                //        .ThenInclude(i => i.Enrollments)
                //            .ThenInclude(i => i.Student)
                //Note: We need to comment out .AsNoTracking method, because Navigation properties can only be explicitly loaded for tracked entities.
                //.AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();
          
            if (id != null)
            {
                InstructorID = id.Value;
                //Instructor instructor = InstructorData.Instructors
                //    .Where(i => i.ID == id.Value).Single();
                //Note: this is a personal preferance. Either way, there is no benefit.
                Instructor instructor = InstructorData.Instructors.Single(i => i.ID == id.Value);

                //Note: Single method is called to convert the collection into a single Instructor entity.
                //      Single method is used on a collection when the collection has only one item.
                //      The single method throws an exception if the collection is empty or if there's more than one item.
                //      Alternative method = "SingleOrDefault" which returns a default value(null in this case) if the collection is empty.
                InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .Where(x => x.CourseID == courseID).Single();
                //Explicit Loading example.
                //Suppose users rarely wnat to see enrollments in a course. 
                //In that case, an optimization would be to only load the enrollment data if it's requested.
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }

        //public IList<Instructor> Instructor { get;set; }

        //public async Task OnGetAsync()
        //{
        //    Instructor = await _context.Instructors.ToListAsync();
        //}
    }
}
