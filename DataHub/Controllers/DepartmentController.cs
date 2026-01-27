using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Data.SqlClient;
using StudentService.BLL;
using StudentService.Models;
using System.Runtime.InteropServices;
using SystemAdmin.Services.SystemClasses;

namespace DataHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet("GetDepartment/{id:int}")]
        public IActionResult GetDepartment([FromRoute]int id = 0)
        {
            try
            {
                DepartmentBLL departmentBLL = new DepartmentBLL();

                var res = departmentBLL.GetDepartment(id);

                if (res == null || res.Count <= 0)
                    return Ok(HelperFunctions.Failure<object>(res, "No department found!"));

                return Ok(HelperFunctions.Success<List<GetDepartmentResponse>>(res, "Department fetched successfully!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in controller GetDepartment: " + ex.ToString());
                return StatusCode(500, HelperFunctions.Failure("Internal server error"));
            }
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody]AddDepartmentRequest request)
        {
            try
            {
                DepartmentBLL departmentBLL = new DepartmentBLL();
                int did = departmentBLL.AddDepartment(request);

                if (did == null || did <= 0)
                    return Ok(HelperFunctions.Failure(did, "Failed to add department! 😊"));

                return Ok(HelperFunctions.Success(did, "Department added successfully! 😕"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in controller AddDepartment: " + ex.ToString());
                return StatusCode(500, HelperFunctions.Failure("Internal server error 😫"));
            }
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody]UpdateDepartmentRequest request)
        {
            try
            {
                DepartmentBLL departmentBLL = new DepartmentBLL();

                bool isUpdated = departmentBLL.UpdateDepartment(request);

                if (isUpdated)
                    return Ok(HelperFunctions.Success(isUpdated, "Department updated successfully 😊"));

                return Ok(HelperFunctions.Failure(isUpdated, "Seems like department doesn't exist 😕"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in controller UpdateDepartment: " + ex.ToString());
                return StatusCode(500, HelperFunctions.Failure("Internal server error 😫"));
            }
        }

        [HttpDelete("DeleteDepartment/{did:int}")]
        public IActionResult DeleteDepartment([FromRoute]int did)
        {
            try
            {
                if(did == null || did <= 0)
                {
                    return Ok(HelperFunctions.Failure(false, "Invalid department id 😕"));
                }

                DepartmentBLL departmentBLL = new DepartmentBLL();

                bool isDeleted = departmentBLL.DeleteDepartment(did);

                if (isDeleted)
                    return Ok(HelperFunctions.Success(isDeleted, "Department deleted successfully 😊"));

                return Ok(HelperFunctions.Failure(isDeleted, "Seems like department doesn't exist 😕"));
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in controller DeleteDepartment: " + ex.ToString());
                return StatusCode(500, HelperFunctions.Failure("Internal server error 😫"));
            }
        }

        [HttpGet("GetDepartmentName")]
        public IActionResult GetDepartmentNameByStudentId([FromQuery]int studentId)
        {
            try
            {
                DepartmentBLL departmentBLL = new DepartmentBLL();

                var res = departmentBLL.GetDepartmentNameByStudentId(studentId);

                if (res != null && res.Count > 0)
                    return Ok(HelperFunctions.Success(res, "Department name retrieved successfully 😊"));

                return Ok(HelperFunctions.Failure(res, "Failed to retrieve department name 😕"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in controller GetDepartmentNameByStudentId: " + ex.ToString());
                return StatusCode(500, HelperFunctions.Failure("Internal server error 😫"));
            }
        }
    }
}