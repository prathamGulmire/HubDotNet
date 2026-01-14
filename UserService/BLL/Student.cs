using StudentService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Services.SystemClasses;
using UserService.Services;

namespace StudentService.BLL
{
    public class StudentBLL
    {
        public IEnumerable<GetAllRecordsResponse> GetUser(int id)
        {
            try
            {
                dbHandler dbHandler = new dbHandler();
                Student student = new Student();
                DataTable dt = new DataTable();
                dt = student.GetAllRecords(dbHandler, id);
                List<GetAllRecordsResponse> res = dt.DataTableToList<GetAllRecordsResponse>();

                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL GetUSer: ", ex.ToString());
                throw;
            }
        }

        public int AddRecord(AddUSerDb addUSer)
        {
            try
            {
                dbHandler db = new dbHandler();
                Student student = new Student();

                int id = student.AddRecordService(db, addUSer);

                return id;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL AddRecord: " + ex.ToString());
                throw;
            }
        }

        public bool UpdateRecord(UpdateUserDb updateUser)
        {
            try
            {
                dbHandler db = new dbHandler();
                Student student = new Student();

                bool isUpdated = student.UpdateRecordService(db, updateUser);

                return isUpdated;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL UpdateRecord: " + ex.ToString());
                throw;
            }
        }

        public bool DeleteRecord(int id)
        {
            try
            {
                dbHandler db = new dbHandler();
                Student student = new Student();

                bool deleted = student.DeleteRecordService(db, id);

                return deleted;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in BLL DeleteRecord: " + ex.ToString());
                throw;
            }
        }

        public IEnumerable<LoginResponse> Login(LoginStudent login)
        {
            try
            {
                dbHandler db = new dbHandler();
                Student student = new Student();
                DataTable dt = new DataTable();

                dt = student.Login(db, login);
                List<LoginResponse> res = dt.DataTableToList<LoginResponse>();

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in BLL login: " + ex.ToString());
                throw;
            }
        }
    }
}