using CST326.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CST326.DAO
{
    public class EmployeeDAO
    {
        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string addUserQry = "insert into Employees (FirstName, LastName, PhoneNumber, Email, Password, IsAdmin) values (@FirstName, @LastName, @PhoneNumber, @Email, @Password, @Admin)";

        string authenticateQry = "select EmployeeId as 'EmployeeId', FirstName as 'FirstName', LastName as 'LastName', EmailAddress as 'Email' from employees where EmployeeId = @employeeId and Password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";


        public bool AddEmployee(EmployeeModel employee)
        {
            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(addUserQry, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Password", employee.Password);
                    cmd.Parameters.AddWithValue("@Admin", employee.Admin);


                    try
                    {
                        conn.Open();
                        int results = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (results == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();
                        throw new Exception("An error occured adding employee to the employees table.\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                    }
                }
            }
        }

        public EmployeeModel Authenticate(EmployeeModel employee)
        {
            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(authenticateQry, conn))
                {
                    cmd.Parameters.AddWithValue("@employeeId", employee.EmployeeId);
                    cmd.Parameters.AddWithValue("@password", employee.Password);

                    try
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            EmployeeModel emp = new EmployeeModel();
                            emp.EmployeeId = (int)reader["EmployeeId"];
                            emp.FirstName = reader["FirstName"].ToString();
                            emp.LastName = reader["LastName"].ToString();
                            emp.Email = reader["Email"].ToString();

                            conn.Close();
                            return emp;
                        }
                        else
                        {
                            conn.Close();
                            return new EmployeeModel();
                        }

                    }
                    catch (SqlException ex)
                    {
                        conn.Close();
                        throw new Exception("An error occured authenticating user.\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                    }
                }
            }

        }
    }
}
