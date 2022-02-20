using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using CST326.Models;

namespace CST326.DAO
{
    public class ProductDAO
    {
        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string CreateProductQuery = "insert into products ([name], [description], [price], [category], [ImageLocation]) values (@name, @description, @price, @category, @imagelocation)";


        public bool CreateProduct(ProductModel product)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(CreateProductQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@category", product.Category.ToString());
                    cmd.Parameters.AddWithValue("@imagelocation", product.ProductImageLocation);
                    
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
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();
                        throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
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