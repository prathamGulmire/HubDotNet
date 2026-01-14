using Microsoft.Data.SqlClient;
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
    public class ManageStudentCourse
    {
        string query = "";

        public void AssignCourse(dbHandler db, AssignCourseRequest assignCourse)
        {
            query = @"
                INSERT INTO StudentCourse (Sid, Cid)
                SELECT @Sid, @Cid
                WHERE NOT EXISTS (
                    SELECT 1 FROM StudentCourse
                    WHERE Sid = @Sid AND Cid = @Cid
                )";

            try
            {
                foreach(var cid in assignCourse.CourseIds)
                {
                    SqlParameter[] sqls =
                    {
                        new SqlParameter("@Sid", assignCourse.Sid),
                        new SqlParameter("@Cid", cid)
                    };

                    db.ExecuteNonQueryData(query, sqls);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service AssignCourse:", ex.ToString());
                throw;
            }
        }

        public DataTable GetCoursesByStudentId(dbHandler db, int id)
        {
            query = "select Cid from StudentCourse where Sid="+id;

            try
            {
                return db.GetData(query);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service GetCoursesByStudentId: ", ex.ToString());
                throw;
            }
        }

        public bool UnassignCoursesByStudentId(dbHandler db, int studentId, List<int> courseIds)
        {
            if (courseIds == null || courseIds.Count == 0)
                return false;

            string query = @"
                        DELETE FROM StudentCourse
                        WHERE Sid = @Sid AND Cid IN (" + string.Join(",", courseIds) + ")";

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Sid", studentId)
                };

                int rows = db.ExecuteNonQueryData(query, parameters);
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UnassignCoursesByStudentId: " + ex);
                throw;
            }
        }

    }
}
