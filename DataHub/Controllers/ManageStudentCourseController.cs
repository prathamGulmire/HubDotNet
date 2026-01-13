using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentService.BLL;
using StudentService.Models;

namespace DataHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageStudentCourseController : ControllerBase
    {
        ManageStudentCourseBLL manageStudentCourseBLL = new ManageStudentCourseBLL();

        [HttpPost]
        [Route("AssignCourse")]
        public IActionResult AssignCourse([FromBody]AssignCourseRequest request)
        {
            try
            {
                if (request == null || request.CourseIds == null || !request.CourseIds.Any())
                    return BadRequest("Invalid request");

                manageStudentCourseBLL.AssignCourse(request);

                return Ok(new
                {
                    success = true,
                    message = "Courses assigned successfully!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in controller AssignCourseController: ", ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("coursesBySid")]
        public IActionResult GetCoursesBySid(int sid)
        {
            try
            {
                if (sid == null || sid <= 0)
                {
                    return BadRequest("Invalid student id.");
                }

                IEnumerable<int> res = manageStudentCourseBLL.GetCoursesByStudentId(sid);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in controller GetCoursesBySid: ", ex.ToString());
                throw;
            }
        }
    }
}
