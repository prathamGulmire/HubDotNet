using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentService.Models;
using System.Data;
using SystemAdmin.Services.SystemClasses;

namespace UserService.Services
{
    public class Student
    {
        string conStr = "Data Source=DESKTOP-27TN82P;Initial Catalog=GCEK;Persist Security Info=True;User ID=sa;Password=12345678;Trust Server Certificate=True;";
        string query = "";

        public  DataTable GetAllRecords(dbHandler dbHandler,  int id = 0)
        {
            List<GetAllRecordsResponse> records = new List<GetAllRecordsResponse>();
            query = "select * from Mytable ";

            try
            {
                if (id > 0)
                {
                    query += "where id=" + id;
                }
                return dbHandler.GetData(query);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service User: ", ex.Message.ToString());
                throw;
            }
        }

        public int AddRecordService(dbHandler dbHandler, AddUSer addUser)
        {
            string query = @"
                INSERT INTO Mytable
                (
                    FirstName,
                    MiddleName,
                    LastName,
                    Email,
                    Gender,
                    DateOfBirth,
                    Address,
                    Country,
                    State,
                    Pincode,
                    Password
                )
                VALUES
                (
                    @FirstName,
                    @MiddleName,
                    @LastName,
                    @Email,
                    @Gender,
                    @DateOfBirth,
                    @Address,
                    @Country,
                    @State,
                    @Pincode,
                    @Password
                );
                SELECT SCOPE_IDENTITY();";

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@FirstName", addUser.FirstName),
                    new SqlParameter("@MiddleName", (object)addUser.MiddleName ?? DBNull.Value),
                    new SqlParameter("@LastName", addUser.LastName),
                    new SqlParameter("@Email", addUser.Email),
                    new SqlParameter("@Gender", addUser.Gender),
                    new SqlParameter("@DateOfBirth", addUser.DateOfBirth),
                    new SqlParameter("@Address", addUser.Address),
                    new SqlParameter("@Country", addUser.Country),
                    new SqlParameter("@State", addUser.State),
                    new SqlParameter("@Pincode", addUser.Pincode),
                    new SqlParameter("@Password", addUser.password)
                };

                object result = dbHandler.ExecuteScalarData(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in service AddRecord: " + ex.Message);
                throw;
            }
        }

        public bool UpdateRecordService(dbHandler dbHandler, UpdateUser updateUser)
        {
            string query = @"
                UPDATE Mytable
                SET
                    FirstName   = @FirstName,
                    MiddleName  = @MiddleName,
                    LastName    = @LastName,
                    Email       = @Email,
                    Gender      = @Gender,
                    DateOfBirth = @DateOfBirth,
                    Address     = @Address,
                    Country     = @Country,
                    State       = @State,
                    Pincode     = @Pincode,
                    UpdatedAt   = @UpdatedAt
                WHERE Id = @Id";

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", updateUser.Id),
                    new SqlParameter("@FirstName", updateUser.FirstName),
                    new SqlParameter("@MiddleName", (object)updateUser.MiddleName ?? DBNull.Value),
                    new SqlParameter("@LastName", updateUser.LastName),
                    new SqlParameter("@Email", updateUser.Email),
                    new SqlParameter("@Gender", updateUser.Gender),
                    new SqlParameter("@DateOfBirth", updateUser.DateOfBirth),
                    new SqlParameter("@Address", updateUser.Address),
                    new SqlParameter("@Country", updateUser.Country),
                    new SqlParameter("@State", updateUser.State),
                    new SqlParameter("@Pincode", updateUser.Pincode),
                    new SqlParameter("@UpdatedAt", DateTime.Now)
                };

                int rowsAffected = dbHandler.ExecuteNonQueryData(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in service UpdateRecord: " + ex.Message);
                throw;
            }
        }

        public bool DeleteRecordService(dbHandler db, int id)
        {
            query = "DELETE FROM Mytable WHERE id = @id";

            try
            {
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@id", id)
                };

                int rows = db.ExecuteNonQueryData(query, sqlParameters);

                return rows > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in service DeleteRecord: " + ex.Message);
                throw;
            }
        }

        public int? Login(dbHandler db, LoginStudent loginStudent)
        {
            string query = @"SELECT id 
                     FROM Mytable 
                     WHERE email = @email AND password = @password";

            try
            {
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@email", loginStudent.email),
                    new SqlParameter("@password", loginStudent.password)
                };

                object result = db.ExecuteScalarData(query, sqlParameters);

                if (result != null)
                    return Convert.ToInt32(result);

                return null; // invalid login
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Login: " + ex);
                throw;
            }
        }
    }
}