using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {

        public List<AssignedCourseData> AssignedCourseDataList;

        public void PopulateAssignedCourseData(SchoolContext context,
                                               Instructor instructor)
        {
            var allCourses = context.Courses;
            var instructorCourses = new HashSet<int>(
                instructor.Courses.Select(c => c.ID));
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    CourseID = course.ID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.ID)
                });
            }
        }

        public void UpdateInstructorCourses(SchoolContext context,
            string[] selectedCourses, Instructor instructorToUpdate)
        {
            //if (selectedCourses == null)  官方demo的写法有bug,改为如下即可
            if (selectedCourses.Length == 0)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            //这里干嘛要一个hashset<string> 一个int?,知道了,这里是前端模型绑定传过来的,本来就是string数组
            //两个HashSet,只用了一次循环就解决了问题,值得学习..主要还是linq好用,我一般是写一个循环拿到courseid放到集合里面
            //而这里是用linq写法,放到hashset里面,hashset效率更高,linq写法更简洁
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.ID));
            foreach (var course in context.Courses)
            {
                //将选中的课加到该老师的课里面
                if (selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!instructorCourses.Contains(course.ID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    //else 未选中的课,则从老师的课里面移除
                    if (instructorCourses.Contains(course.ID))
                    {
                        Course courseToRemove
                            = instructorToUpdate
                                .Courses
                                .SingleOrDefault(i => i.ID == course.ID);
                        instructorToUpdate.Courses.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}