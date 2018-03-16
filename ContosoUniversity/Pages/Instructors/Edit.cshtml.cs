using System.Threading.Tasks;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors
{
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InstructorViewModel Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await new InstructorMapper().ManyTo(_context.Instructors).FirstOrDefaultAsync(m => m.Id == id);

            if (Instructor == null)
            {
                return NotFound();
            }

            var allCourses = new CourseMapper().ManyTo(_context.Courses);

            PopulateAssignedCourseData(allCourses, Instructor);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var instructorToUpdate = await new InstructorMapper().ManyTo(_context.Instructors).FirstOrDefaultAsync(i => i.Id == id);           

            if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
            {
                instructorToUpdate.OfficeAssignment = null;
            }

            var allCourses = new CourseMapper().ManyTo(_context.Courses);

            UpdateInstructorCourses(allCourses, selectedCourses, instructorToUpdate);

            instructorToUpdate.LastName = Instructor.LastName;
            instructorToUpdate.FirstMidName = Instructor.FirstMidName;
            instructorToUpdate.HireDate = Instructor.HireDate;
            instructorToUpdate.OfficeAssignment = string.IsNullOrWhiteSpace(Instructor.OfficeAssignment.Location) ? null : Instructor.OfficeAssignment;

            _context.Instructors.Update(new InstructorMapper().SingleFrom(instructorToUpdate));

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
