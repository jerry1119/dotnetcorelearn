using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;  // Add VM

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
                .Include(i => i.Courses)
                    .ThenInclude(c => c.Department)
                // .Include(i => i.Courses)
                //     .ThenInclude(c => c.Enrollments)
                //         .ThenInclude(e => e.Student)
                //.AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();  //tolist是直接把这几张表全加载到内存中了
            
            if (id is not null)
            {
                InstructorID = id.Value;  //这个.value值得学习
                Instructor instructor = InstructorData.Instructors
                    .Single(i => i.ID == id.Value); //SingleorDefault或许好一点？
                InstructorData.Courses = instructor.Courses;
            }
            if (courseID != null)
            {
                courseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .Where(c => c.ID == courseID).SingleOrDefault();//直接向上面那样single也可以，但是可读性差一点
                await _context.Entry(selectedCourse).Collection(c  => c.Enrollments).LoadAsync();
                foreach (var enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(e => e.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}
