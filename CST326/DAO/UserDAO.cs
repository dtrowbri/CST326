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

        string authenticateQry = "select User_Id as 'UserId', First_Name as 'FirstName', Last_Name as 'LastName', Email as 'Email' from users where email = @email and password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";

        string isEmailValidQry = "select count(*) as 'Count' from [Users] where [Email] = @Email";

        string setNewPasswordQry = "update [Users] set [Password] = @password where [Email] = @Email";

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
                        conn.Close();
                        throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                    } catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                    }
                }
            }
        }

        public UserModel Authenticate(UserModel user)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(authenticateQry, conn))
                {
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    
                    try
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            UserModel customer = new UserModel();
                            customer.UserId = (int)reader["UserId"];
                            customer.FirstName = reader["FirstName"].ToString();
                            customer.LastName = reader["LastName"].ToString();
                            customer.Email = reader["Email"].ToString();

                            conn.Close();
                            return customer;
                        }
                        else
                        {
                            conn.Close();
                            return new UserModel();
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
 
        public bool isEmailValid(UserModel user)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(isEmailValidQry, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            if((int)reader["Count"] == 1)
                            {
                                conn.Close();
                                return true;
                            } else
                            {
                                conn.Close();
                                return false;
                            }
                        } else
                        {
                            conn.Close();
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("There was an error validating the user's email. Ex: " + ex.Message);
                    } 
                    catch(Exception ex)
                    {
                        throw new Exception("There was an unexpected error validate the user's email. Ex: " + ex.Message);
                    }
                }
            }
        }
    
        public bool setNewPassword(UserModel user)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(setNewPasswordQry, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@password", user.Password);
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
                    } catch (SqlException ex)
                    {
                        throw new Exception("There was an error updating the password. Ex: " + ex.Message);
                    } catch (Exception ex)
                    {
                        throw new Exception("There was an unexpected error updating the password. Ex: " + ex.Message);
                    }
                }
            }

        }

    }
}