using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using StudentService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Services.SystemClasses;

namespace StudentService.Services
{
    public class DepartmentService
    {
        string query = "";
        public DataTable GetDepartment(dbHandler db, int did = 0)
        {
            query = "select * from Department";

            try
            {
                SqlParameter[] parameters = { };

                if (did > 0)
                {
                    query += " where DepartmentId = @DepartmentId";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@DepartmentId", did)
                    };
                }

                return db.GetDataWithParams(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in service GetDepartment: ", ex.ToString());
                throw;
            }
        }

        public int AddDepartment(dbHandler db, AddDepartmentRequest request)
        {
            try
            {
                query = "insert into Department(DepartmentName) values(@DepartmentName); SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentName", request.DepartmentName)
                };

                object result = db.ExecuteScalarData(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in service AddDepartment: ", ex.ToString());
                throw;
            }
        }

        public bool UpdateDepartment(dbHandler db, UpdateDepartmentRequest request)
        {
            try
            {
                query = "update Department set DepartmentName = @DepartmentName, UpdatedAt=@UpdatedAt where DepartmentId = @DepartmentId";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentId", request.DepartmentId),
                    new SqlParameter("@DepartmentName", request.DepartmentName),
                    new SqlParameter("@UpdatedAt", DateTime.Now)
                };

                int rows = db.ExecuteNonQueryData(query, parameters);

                return rows > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service UpdateDepartment: ", ex.ToString());
                throw;
            }
        }

        public bool DeleteDepartment(dbHandler db, int did)
        {
            try
            {
                query = "delete from Department where DepartmentId = @DepartmentId";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentId", did)
                };

                int rows = db.ExecuteNonQueryData(query, parameters);

                return rows > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service DeleteDepartment: ", ex.ToString());
                throw;
            }
        }

        public DataTable GetDepartmentNameByStudentId(dbHandler db, int studentId)
        {
            try
            {
                query = @"
                    select s.id, s.firstName, d.DepartmentId, d.DepartmentName
                    from Mytable s
                    left join Department d
                    on s.DepartmentId = d.DepartmentId
                    where s.id = @id;";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@id", studentId)
                };

                return db.GetDataWithParams(query, parameters);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service GetDepartmentNameByStudentId: ", ex.ToString());
                throw;
            }
        }

        public int GetCountOfStudentsInDepartment(dbHandler db, int did)
        {
            try
            {
                query = @"select COUNT(s.firstName) 'no. of students in department'
                          from Department d 
                          left join Mytable s
                          on d.DepartmentId = s.DepartmentId
                          where d.DepartmentId = @DepartmentId
                          group by d.DepartmentId;
                        ";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@DepartmentId", did)
                };

                object countOfStudents = db.ExecuteScalarData(query, parameters);

                return Convert.ToInt32(countOfStudents);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service GetCountOfStudentsInDepartment!", ex.ToString());
                throw;
            }
        }
    }
}