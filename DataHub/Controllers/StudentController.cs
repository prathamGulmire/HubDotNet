using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentService.BLL;
using StudentService.Models;
using UserService.Services;

namespace DataHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        StudentBLL studentBLL = new StudentBLL();

        [HttpGet]
        [Route("getStudents/{id:int}")]
        public IActionResult GetAllRecords([FromRoute] int id)
        {
            var res = studentBLL.GetUser(id);
            //Console.WriteLine(msg);
            return Ok(res);
        }

        [HttpPost]
        [Route("addRecord")]
        public IActionResult AddRecord([FromBody] AddUSer addUser)
        {
            try
            {
                if (addUser == null)
                    return BadRequest(new { success = false, message = "Invalid user data." });

                int id = studentBLL.AddRecord(addUser);

                return Ok(new
                {
                    success = true,
                    message = "User added successfully",
                    studentId = id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddRecord controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPut("UpdateRecord")]
        public IActionResult UpdateRecord([FromBody] UpdateUser updateUser)
        {
            try
            {
                if (updateUser == null || updateUser.Id <= 0)
                {
                    return BadRequest("Invalid user data.");
                }

                bool updated = studentBLL.UpdateRecord(updateUser);

                if (updated)
                    return Ok(new { success = true, message = "User updated successfully" });
                else
                    return NotFound(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateUser controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete]
        [Route("deleteRecord/{id:int}")]
        public IActionResult DeleteRecord([FromRoute]int id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return BadRequest("Invalid id.");
                }

                bool deleted = studentBLL.DeleteRecord(id);

                if (deleted)
                    return Ok(new { success = true, message = "Student deleted successfully" });
                else
                    return NotFound(new { success = false, message = "Student not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateUser controller: " + ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginStudent login)
        {
            int? studentId = studentBLL.Login(login);

            if (studentId == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new
            {
                message = "Login successful",
                studentId = studentId
            });
        }
    }
}