using Microsoft.Data.SqlClient;
using StudentService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Services.SystemClasses;
using System.Data;

namespace StudentService.Services
{
    public class Course
    {

        string query = "";

        public DataTable GetAllCourses(dbHandler db, int id = 0)
        {
            query = "select * from Course";

            try
            {
                if (id > 0)
                {
                    query += " where CourseId = " + id;
                }

                return db.GetData(query);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service getallCourses: ", ex.Message.ToString());
                throw;
            }
        }

        public int AddCourse(dbHandler db, AddCourse addCourse)
        {
            query = @"insert into Course (CourseCode, CourseName, Description, DurationMonths) values " +
                "(@CourseCode, @CourseName, @Description, @DurationMonths); SELECT SCOPE_IDENTITY();";

            try
            {
                SqlParameter[] sp =
                {
                    new SqlParameter("@CourseCode", addCourse.CourseCode),
                    new SqlParameter("@CourseName", addCourse.CourseName),
                    new SqlParameter("@Description", addCourse.Description),
                    new SqlParameter("@DurationMonths", addCourse.DurationMonths)
                };

                object result = db.ExecuteScalarData(query, sp);
                return Convert.ToInt32(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service AddCourse: ", ex.Message.ToString());
                throw;
            }
        }

        public bool UpdateCourse(dbHandler db, UpdateCourse updateCourse)
        {
            query = "update Course set CourseCode=@CourseCode, CourseName=@CourseName, Description=@Description, " +
                "DurationMonths=@DurationMonths, UpdatedAt=@UpdatedAt where CourseId=@CourseId";

            try
            {
                SqlParameter[] sp =
                {
                    new SqlParameter("@CourseId", updateCourse.CourseId),
                    new SqlParameter("@CourseCode", updateCourse.CourseCode),
                    new SqlParameter("@CourseName", updateCourse.CourseName),
                    new SqlParameter("@Description", updateCourse.Description),
                    new SqlParameter("@DurationMonths", updateCourse.DurationMonths),
                    new SqlParameter("@UpdatedAt", DateTime.Now)
                };

                int rows = db.ExecuteNonQueryData(query, sp);

                return rows > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service UpdateCourse: ", ex.Message.ToString());
                throw;
            }
        }

        public bool DeleteCourse(dbHandler db, int CourseId)
        {
            try
            {
                query = "select count(1) from StudentCourse where Cid=@Cid";

                SqlParameter[] sqls =
                {
                    new SqlParameter("@Cid", CourseId)
                };

                int count = Convert.ToInt32(db.ExecuteScalarData(query, sqls));

                if(count > 0)
                {
                    return false;
                }

                query = "delete from Course where CourseId=@CourseId";

                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@CourseId", CourseId),
                };

                int rows = db.ExecuteNonQueryData(query, sqlParameters);

                return rows > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service DeleteCourse: ", ex.Message.ToString());
                throw;
            }
        }
    }
}