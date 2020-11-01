using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]  //razor 就是通过这个来绑定页面和cs代码的，我就说怎么传递的数据
        public Student Student { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyStudent = new Student();
            // 这么搞是为了防止过多发布攻击，强制就只接收这3个属性，
            //一般是做法搞个dto或者viewmodel(但是这里是razor(asp)页面，需要BindProperty和cshtml页面也改成对应model才行)。
            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",    //查找带有student前缀的表单值，如student.LastName 该自变量不区分大小写
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
