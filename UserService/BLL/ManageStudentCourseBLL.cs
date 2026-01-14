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
    public class ManageStudentCourseBLL
    {
        public void AssignCourse(AssignCourseRequest assignCourse)
        {
            using (dbHandler db = new dbHandler(withTransaction: true))
            {
                ManageStudentCourse manage = new ManageStudentCourse();

                try
                {
                    manage.AssignCourse(db, assignCourse);
                    db.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in BLL AssignCourse: ", ex.ToString());
                    db.RollBack();
                }
            }
                
        }

        public IEnumerable<int> GetCoursesByStudentId(int id)
        {
            try
            {
                using (dbHandler db = new dbHandler())
                {
                    DataTable dt = new DataTable();
                    ManageStudentCourse manageStudentCourse = new ManageStudentCourse();

                    dt = manageStudentCourse.GetCoursesByStudentId(db, id);
                    List<int> res = dt.AsEnumerable()
                                  .Select(row => Convert.ToInt32(row["Cid"]))
                                  .ToList();

                    return res;
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL GetCoursesByStudentId: ", ex.ToString());
                throw;
            }
        }

        public bool UnassignCourses(int studentId, List<int> courseIds)
        {
            using (dbHandler db = new dbHandler(true)) // transaction enabled
            {
                ManageStudentCourse service = new ManageStudentCourse();

                try
                {
                    bool result = service.UnassignCoursesByStudentId(db, studentId, courseIds);
                    db.Commit();
                    return result;
                }
                catch
                {
                    db.RollBack();
                    throw;
                }
            } 
        }
    }
}
