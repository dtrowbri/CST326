using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using CST326.Models;
using System.Data.SqlClient;

namespace CST326.DAO
{
    public class UserDAO
    {

        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string addUserQry = "insert into Users (First_Name, Last_Name, Email, Password) values (@FirstName, @LastName, @Email, @Password)";


        public bool AddUser(UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(addUserQry, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);


                    try
                    {
                        conn.Open();
                        int results = cmd.ExecuteNonQuery();
                        conn.Close();

                        if(results == 1)
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    } catch (SqlException ex) {
                        throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                    } catch (Exception ex)
                    {
                        throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                    }
                }
            }

        }
    }
}