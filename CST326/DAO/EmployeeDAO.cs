using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using CST326.Models;
using System.Data.SqlClient;

namespace CST326.DAO
{
    public class EmployeeDAO
    {
        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string addUserQry = "insert into Employees (Employee_Id, First_Name, Last_Name, Phone_Number, Email, Password) values (@FirstName, @LastName, @PhoneNumber, @Email, @Password)";

        string authenticateQry = "select count(1) as 'count' from employees where employee_id = @employeeId and password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";


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

        public bool Authenticate(EmployeeModel employee)
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
                        int count = 0;

                        if (reader.HasRows)
                        {
                            reader.Read();
                            count = (int)reader["count"];
                            conn.Close();
                        }
                        else
                        {
                            return false;
                        }

                        if (count > 0)
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