using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        // public IList<Student> Students { get; set; }
        public PaginatedList<Student> Students { get; set; }

        public async Task OnGetAsync(string sortOrder,
        string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                // reset to 1 because the new filter can result in different data to display.
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                //Note: Where method on an IQueryable object, and the filter is processed on the server.
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            //Note: IQueryable are converted to a collection by calling a method such as ToListAsync.
            //Students = await studentsIQ.AsNoTracking().ToListAsync();

            int pageSize = 3;
            Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            // pageIndex ?? 1 returns the value of pageIndex if it has a value, otherwise, it returns 1.
        }


        //private readonly ContosoUniversity.Data.SchoolContext _context;

        //public IndexModel(ContosoUniversity.Data.SchoolContext context)
        //{
        //    _context = context;
        //}

        //public IList<Student> Student { get;set; }

        //public async Task OnGetAsync()
        //{
        //    Student = await _context.Students.ToListAsync();
        //}
    }
}
