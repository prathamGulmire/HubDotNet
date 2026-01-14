using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentService.BLL;
using StudentService.Models;
using StudentService.Services;

namespace DataHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        CourseBLL courseBLL = new CourseBLL();

        [HttpGet("GetCourse/{courseId:int}")]
        public IActionResult GetCourse([FromRoute]int courseId)
        {
            try
            {
                if (courseId == null)
                {
                    return BadRequest("Invalid course id.");
                }

                var res = courseBLL.GetCourseRecord(courseId);

                return Ok(res);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in GetCourse controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourse([FromBody]AddCourse add)
        {
            try
            {
                if (add == null)
                    return BadRequest(new { success = false, message = "Invalid course details." });

                int id = courseBLL.AddCourseRecord(add);

                return Ok(new
                {
                    success = true,
                    message = "Course added successfully",
                    courseId = id
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in AddCourse controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPut("UpdateCourse")]
        public IActionResult UpdateCourse([FromBody]UpdateCourse course)
        {
            try
            {
                if (course == null || course.CourseId <= 0)
                {
                    return BadRequest("Invalid course details.");
                }

                bool updated = courseBLL.UpdateCourseRecord(course);

                if (updated)
                    return Ok(new { success = true, message = "Course updated successfully" });
                else
                    return NotFound(new { success = false, message = "Course not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateCourse controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("DeleteCourse/{courseID:int}")]
        public IActionResult DeleteCourseRecord([FromRoute]int courseID)
        {
            try
            {
                if(courseID <= 0)
                {
                    return BadRequest("Invalid courseId!");
                }

                bool isDeleted = courseBLL.DeleteCourseRecord(courseID);

                if (isDeleted)
                    return Ok(new { success = true, message = "Course deleted successfully" });
                else
                    return Ok(new { success = false, message = "Course not found" });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in DeleteCourseRecord controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}