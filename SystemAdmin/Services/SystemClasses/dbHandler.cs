using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SystemAdmin.Services.SystemClasses
{
    public class dbHandler: IDisposable
    {
        private readonly SqlTransaction _transaction;
        private readonly SqlConnection _connection;
        int intTimeOutPeriod = 50000;
        private readonly bool ExecuteWithTransaction = false;

        public SqlConnection Connection => _connection;
        public SqlTransaction Transaction => _transaction;

        string conStr = "Data Source=DESKTOP-27TN82P;Initial Catalog=GCEK;User ID=sa;Password=12345678;Trust Server Certificate=True";

        public dbHandler(bool withTransaction=false, System.Data.IsolationLevel isolation = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                ExecuteWithTransaction = withTransaction;
                _connection = new SqlConnection(conStr);
                _connection.Open();
                if(ExecuteWithTransaction == true)
                {
                    _transaction = _connection.BeginTransaction(isolation);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in dbHandler constructor: ", ex.ToString());
            }
        }

        public void Commit()
        {
            try
            {
                if (ExecuteWithTransaction && _transaction != null)
                {
                    _transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "Commit Method Problem");
                throw;
            }
        }

        public void RollBack()
        {
            try
            {
                if (ExecuteWithTransaction && _transaction != null)
                {
                    _transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "RollBack Method Problem!");
            }
        }

        public void Dispose()
        {
            try
            {
                _transaction?.Dispose();

                if (_connection?.State == ConnectionState.Open)
                {
                    _connection.Close();
                }

                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "Dispose Method Problem"!);
                throw;
            }
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand(query, _connection);
                if(ExecuteWithTransaction == true)
                {
                    cmd.Transaction = _transaction;
                }
                cmd.CommandTimeout = intTimeOutPeriod;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetData dbHandler: ", ex.ToString());
                throw;
            }
        }

        public object ExecuteScalarData(string query, SqlParameter[] parameters)
        {
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand(query, _connection);

                if (ExecuteWithTransaction == true)
                {
                    cmd.Transaction = _transaction;
                }

                cmd.CommandTimeout = intTimeOutPeriod;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ExecuteScalarData dbHandler: " + ex.ToString());
                throw;
            }
        }

        public int ExecuteNonQueryData(string query, SqlParameter[] parameters = null)
        {
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand(query, _connection);

                if (ExecuteWithTransaction == true)
                {
                    cmd.Transaction = _transaction;
                }

                cmd.CommandTimeout = intTimeOutPeriod;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ExecuteNonQueryData dbHandler: " + ex.ToString());
                throw;
            }
        }
    }
}