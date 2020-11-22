using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<Course>();
            PopulateAssignedCourseData(_context, instructor);
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            Instructor newInstructor = new Instructor();
            if (selectedCourses != null)
            {
                //demo里面这句,好像不加也行,不知道有啥用,好像是初始化导航属性的实例
                //之前那个rupeng项目是直接在entity类那里初始化的
                //知道了,这里的方式是只new自己需要的,entity类那种是将所有导航属性全new了
                newInstructor.Courses = new List<Course>(); 
                foreach (var courseID in selectedCourses)
                {
                    var courseToAdd = _context.Courses
                            .FirstOrDefault(c => c.ID == int.Parse(courseID));
                    newInstructor.Courses.Add(courseToAdd);
                }
            }
            if (await TryUpdateModelAsync<Instructor>(
                newInstructor,
                "Instructor",
                i => i.LastName, i => i.FirstMidName,
                i => i.HireDate, i => i.OfficeAssignment
            ))
            {
                _context.Instructors.Add(newInstructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            //如果添加失败,将已填写的信息填充回去
            PopulateAssignedCourseData(_context, newInstructor);
            return Page();
        }
    }
}
