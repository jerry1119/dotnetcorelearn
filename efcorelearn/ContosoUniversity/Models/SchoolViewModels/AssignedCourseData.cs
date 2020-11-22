namespace ContosoUniversity.Models.SchoolViewModels
{
    //包含的数据可用于为已分配给讲师的课程创建复选框
    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}