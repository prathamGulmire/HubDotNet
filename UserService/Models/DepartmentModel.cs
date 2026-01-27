using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService.Models
{
    public class DepartmentModel
    {
    }

    public class AddDepartmentRequest
    {
        public string DepartmentName { get; set; }
    }

    public class UpdateDepartmentRequest
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class GetDepartmentResponse
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetDepartmentNameByStudentIdRequest
    {
        public int id { get; set; }
    }

    public class GetDepartmentNameByStudentIdResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
