using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService.Models
{
    public class ManageStudentCourseModel
    {
    }

    public class AssignCourseRequest
    {
        public int Sid { get; set; }
        public List<int> CourseIds { get; set; }
    }

    public class GetCoursesByStudentID
    {
        public int Sid { get; set; }
    }
}
