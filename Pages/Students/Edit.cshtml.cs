using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            //if (Student == null)
            //{
            //    return NotFound();
            //}
            //return Page();

            if (id == null)
            {
                return NotFound();
            }
            // Note: When you don't have to include related data, FindAsync is more efficient.
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(Student).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!StudentExists(Student.ID))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return RedirectToPage("./Index");

            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                // Note: The database context keeps track of whether entities in memory are in sync with their corresponding rows in the database.
                //       This tracking information determines what happens when SaveChangesAsync is called.
                /*  States. WOW.
                    Added: The entity doesn't yet exist in the database. The SaveChanges method issues an INSERT statement.
                    Unchanged: No changes need to be saved with this entity. An entity has this status when it's read from the database.
                    Modified: Some or all of the entity's property values have been modified. The SaveChanges method issues an UPDATE statement.
                    Deleted: The entity has been marked for deletion. The SaveChanges method issues a DELETE statement.
                    Detached: The entity isn't being tracked by the database context.
                 */
                // Note: Wow point: In a desktop app, state changes are typically set automatically. An entity is read, changes are made, and the entity state is automatically changed to Modified. 
                //            Calling SaveChanges generates a SQL UPDATE statement that updates only the changed properties.
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
