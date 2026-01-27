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
        string query = "";

        string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public  DataTable GetAllRecords(dbHandler dbHandler,  int id = 0)
        {
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
                Console.WriteLine("Error in service GetAllRecords: ", ex.Message.ToString());
                throw;
            }
        }

        public int AddRecordService(dbHandler dbHandler, AddUSerDb addUser)
        {
            string query = @"
                INSERT INTO Mytable
                (
                    FirstName,
                    MiddleName,
                    LastName,
                    Email,
                    DepartmentId,
                    Gender,
                    DateOfBirth,
                    Address,
                    Country,
                    State,
                    Pincode,
                    Password,
                    imageUrl
                )
                VALUES
                (
                    @FirstName,
                    @MiddleName,
                    @LastName,
                    @Email,
                    @DepartmentId,
                    @Gender,
                    @DateOfBirth,
                    @Address,
                    @Country,
                    @State,
                    @Pincode,
                    @Password,
                    @imageUrl
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
                    new SqlParameter("@DepartmentId", addUser.DepartmentId),
                    new SqlParameter("@DateOfBirth", addUser.DateOfBirth),
                    new SqlParameter("@Address", addUser.Address),
                    new SqlParameter("@Country", addUser.Country),
                    new SqlParameter("@State", addUser.State),
                    new SqlParameter("@Pincode", addUser.Pincode),
                    new SqlParameter("@Password", addUser.password),
                    new SqlParameter("@imageUrl", addUser.imageUrl)
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

        public bool UpdateRecordService(dbHandler dbHandler, UpdateUserDb updateUser)
        {
            query = "select imageUrl from Mytable where id = @id";
            SqlParameter[] param =
            {
                new SqlParameter("@id", updateUser.Id)
            };

            string existingFileName = Convert.ToString(dbHandler.ExecuteScalarData(query, param));

            if(updateUser.imageUrl == null)
            {
                updateUser.imageUrl = existingFileName;
            } 
            else
            {
                string fullPath = Path.Combine(_uploadPath, existingFileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            query = @"
                UPDATE Mytable
                SET
                    FirstName   = @FirstName,
                    MiddleName  = @MiddleName,
                    LastName    = @LastName,
                    Email       = @Email,
                    Gender      = @Gender,
                    DepartmentId= @DepartmentId,
                    DateOfBirth = @DateOfBirth,
                    Address     = @Address,
                    Country     = @Country,
                    State       = @State,
                    Pincode     = @Pincode,
                    UpdatedAt   = @UpdatedAt,
                    Password    = @Password,
                    imageUrl    = @imageUrl 
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
                    new SqlParameter("@DepartmentId", updateUser.DepartmentId),
                    new SqlParameter("@DateOfBirth", updateUser.DateOfBirth),
                    new SqlParameter("@Address", updateUser.Address),
                    new SqlParameter("@Country", updateUser.Country),
                    new SqlParameter("@State", updateUser.State),
                    new SqlParameter("@Pincode", updateUser.Pincode),
                    new SqlParameter("@UpdatedAt", DateTime.Now),
                    new SqlParameter("@Password", updateUser.Password),
                    new SqlParameter("@imageUrl", updateUser.imageUrl)
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
            query = "select count(1) from StudentCourse where Sid=@Sid";

            SqlParameter[] sqls =
            {
                new SqlParameter("@Sid", id)
            };

            int count = Convert.ToInt32(db.ExecuteScalarData(query, sqls));

            if(count > 0)
            {
                return false;
            }

            query = "select imageUrl from Mytable where id = @id";
            SqlParameter[] param =
            {
                new SqlParameter("@id", id)
            };

            string existingFileName = Convert.ToString(db.ExecuteScalarData(query, param));

            string fullPath = Path.Combine(_uploadPath, existingFileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
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

        public DataTable Login(dbHandler db, LoginStudent loginStudent)
        {
            string query = @"SELECT id, role
                     FROM Mytable 
                     WHERE email = @email AND password = @password";
            try
            {
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@email", loginStudent.email),
                    new SqlParameter("@password", loginStudent.password)
                };

                return db.GetDataWithParams(query, sqlParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Login: " + ex);
                throw;
            }
        }

        public void UpdateLastLogin(dbHandler db, int userId)
        {
            string query = @"
                        UPDATE Mytable
                        SET LastLoginAt = GETDATE()
                        WHERE id = @id";

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@id", userId)
            };

            db.ExecuteNonQueryData(query, sqlParameters);
        }

    }
}