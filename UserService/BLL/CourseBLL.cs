using StudentService.Models;
using StudentService.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Services.SystemClasses;

namespace StudentService.BLL
{
    public class CourseBLL
    {
        public IEnumerable<GetCourseRecord> GetCourseRecord(int id)
        {
            try
            {
                using (dbHandler db = new dbHandler())
                {
                    DataTable dt = new DataTable();
                    Course course = new Course();
                    dt = course.GetAllCourses(db, id);

                    List<GetCourseRecord> records = dt.DataTableToList<GetCourseRecord>();

                    return records;
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL GetCourseRecord: ", ex.ToString());
                throw;
            }
        }

        public int AddCourseRecord(AddCourse addCourse)
        {
            try
            {
                using (dbHandler db = new dbHandler())
                {
                    Course course = new Course();

                    int courseId = course.AddCourse(db, addCourse);

                    return courseId;
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL AddCourseRecord: ", ex.ToString());
                throw;
            }
        }

        public bool UpdateCourseRecord(UpdateCourse updateCourse)
        {
            try
            {
                using (dbHandler db = new dbHandler())
                {
                    Course course = new Course();

                    bool isUpdated = course.UpdateCourse(db, updateCourse);

                    return isUpdated;
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL UpdateCourseRecord: ", ex.ToString());
                throw;
            }
        }

        public bool DeleteCourseRecord(int courseId)
        {
            try
            {
                using (dbHandler db = new dbHandler())
                {
                    Course course = new Course();

                    bool isDeleted = course.DeleteCourse(db, courseId);

                    return isDeleted;
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL DeleteCourseRecord: ", ex.ToString());
                throw;
            }
        }
    }
}
