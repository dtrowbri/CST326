using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using CST326.Models;
using System.Collections.Generic;

namespace CST326.DAO
{
    public class ProductDAO
    {
        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string CreateProductQuery = "insert into products ([name], [description], [price], [category], [ImageLocation]) values (@name, @description, @price, @category, @imagelocation)";

        string GetAllProductsQuery = "select [ProductId] as 'ProductId', [Name] as 'Name', [Description] as 'Description', [Price] as 'Price', [Category] as 'Category', [ImageLocation] as 'ImageLocation' from products";

        string GetProductQuery = "select [ProductId] as 'ProductId', [Name] as 'Name', [Description] as 'Description', [Price] as 'Price', [Category] as 'Category', [ImageLocation] as 'ImageLocation' from products where productid = @productid";

        public bool CreateProduct(ProductModel product)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(CreateProductQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@category", product.Category);
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
       
        public List<ProductModel> GetAllProducts()
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(GetAllProductsQuery, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader results = cmd.ExecuteReader();

                        List<ProductModel> ProductList = new List<ProductModel>();

                        while (results.Read())
                        {
                            ProductModel product = new ProductModel();
                            product.ProductId = (int)results["ProductId"];
                            product.Name = results["Name"].ToString();
                            product.Price = Convert.ToDouble(results["Price"]);
                            product.Description = results["Description"].ToString();
                            product.ProductImageLocation = results["ImageLocation"].ToString().Replace('\\', '/');
                            product.Category = (ProductCategory)(int)results["Category"];

                            ProductList.Add(product);
                        }
                        conn.Close();
                        return ProductList;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("An unexpected error occured retrieving the products from the database. Error: " + ex.Message);
                    }
                }
            }
        }
    
        public ProductModel GetProduct(int id)
        {
            using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
                using(SqlCommand cmd = new SqlCommand(GetProductQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@productid", id);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ProductModel product = new ProductModel();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            
                            product.ProductId = (int)reader["ProductId"];
                            product.Description = reader["Description"].ToString();
                            product.Price = Convert.ToDouble(reader["Price"]);
                            product.Category = (ProductCategory)(int)reader["Category"];
                            product.ProductImageLocation = reader["ImageLocation"].ToString();
                            product.Name = reader["Name"].ToString();
                            
                        }
                        conn.Close();
                        return product;
                    } catch (Exception ex)
                    {
                        throw new Exception("There was an error retrieving product with product id {" + id.ToString() + "} from the database. Error: " + ex.Message);
                    }
                }
            }
        }
    }
}