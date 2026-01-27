using StudentService.Models;
using StudentService.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Services.SystemClasses;

namespace StudentService.BLL
{
    public class DepartmentBLL
    {

        private readonly DepartmentService departmentService = new DepartmentService();

        public List<GetDepartmentResponse> GetDepartment(int id)
        {
            try
            {
                using(dbHandler db = new dbHandler())
                {
                    DataTable dt = new DataTable();

                    dt = departmentService.GetDepartment(db, id);

                    return dt.DataTableToList<GetDepartmentResponse>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in BLL GetDepartment: " + ex.ToString());
                throw;
            }
        }

        public int AddDepartment(AddDepartmentRequest request)
        {
            using (dbHandler db = new dbHandler(withTransaction: true))
            {
                try
                {
                    int departmentId = departmentService.AddDepartment(db, request);
                    db.Commit();

                    return departmentId;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error in BLL AddDepartment: ", ex.ToString());
                    db.RollBack();
                    throw;
                }
            }
        }

        public bool UpdateDepartment(UpdateDepartmentRequest request)
        {
            using(dbHandler db = new dbHandler(withTransaction: true))
            {
                try
                {
                    bool isUpdated = departmentService.UpdateDepartment(db, request);
                    db.Commit();

                    return isUpdated;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error in BLL UpdateDepartment: ", ex.ToString());
                    db.RollBack();
                    throw;
                }
            }
        }

        public bool DeleteDepartment(int did)
        {
            using(dbHandler db = new dbHandler(withTransaction: true))
            {
                try
                {
                    bool isDeleted = departmentService.DeleteDepartment(db, did);
                    db.Commit();

                    return isDeleted;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error in BLL DeleteDepartment: ", ex.ToString());
                    db.RollBack();
                    throw;
                }
            }
        }

        public List<GetDepartmentNameByStudentIdResponse> GetDepartmentNameByStudentId(int studentId)
        {
            try
            {
                using(dbHandler db = new dbHandler())
                {
                    DataTable dt = departmentService.GetDepartmentNameByStudentId(db, studentId);
                    var response = dt.DataTableToList<GetDepartmentNameByStudentIdResponse>();

                    return response;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL GetDepatmentNameByStudentId: ", ex.ToString());
                throw;
            }
        }
    }
}
