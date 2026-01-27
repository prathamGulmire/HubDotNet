using DataHub.Common;
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
        public IActionResult AddRecord([FromForm] AddUSer addUser)
        {
            try
            {
                if (addUser == null)
                    return BadRequest(new { success = false, message = "Invalid user data." });

                ImageService imageService = new ImageService();
                AddUSerDb addUserDb = new AddUSerDb
                {
                    FirstName = addUser.FirstName,
                    MiddleName = addUser.MiddleName,
                    LastName = addUser.LastName,
                    Email = addUser.Email,
                    DepartmentId = addUser.DepartmentId,
                    Gender = addUser.Gender,
                    DateOfBirth = addUser.DateOfBirth,
                    Address = addUser.Address,
                    Country = addUser.Country,
                    State = addUser.State,
                    Pincode = addUser.Pincode,
                    password = addUser.password
                };

                if (addUser.imageFile != null)
                {
                    string prefix = addUser.FirstName.Replace(" ", "_");
                    addUserDb.imageUrl = imageService.SaveImage(addUser.imageFile, prefix);
                }

                int id = studentBLL.AddRecord(addUserDb);

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
        public IActionResult UpdateRecord([FromForm] UpdateUser updateUser)
        {
            try
            {
                if (updateUser == null || updateUser.Id <= 0)
                {
                    return BadRequest("Invalid user data.");
                }

                ImageService imageService = new ImageService();
                UpdateUserDb updateUserDb = new UpdateUserDb()
                {
                    Id = updateUser.Id,
                    FirstName = updateUser.FirstName,
                    LastName = updateUser.LastName,
                    Email = updateUser.Email,
                    Gender = updateUser.Gender,
                    DepartmentId = updateUser.DepartmentId,
                    DateOfBirth = updateUser.DateOfBirth,
                    Address = updateUser.Address,
                    Country = updateUser.Country,
                    Pincode = updateUser.Pincode,
                    Password = updateUser.Password,
                    State = updateUser.State,
                    MiddleName = updateUser.MiddleName,
                };

                if(updateUser.imageFile != null && updateUser.imageFile.Length > 0)
                {
                    string prefix = updateUser.FirstName.Replace(" ", "_");
                    updateUserDb.imageUrl = imageService.SaveImage(updateUser.imageFile, prefix);
                } else
                {
                    updateUserDb.imageUrl = null;
                }

                bool updated = studentBLL.UpdateRecord(updateUserDb);

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
                    return Ok(new { success = false, message = "Courses are assigned to students" });
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
            var res = studentBLL.Login(login);

            if (res == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(res);
        }
    }
}