using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using CST326.Models;

namespace CST326.DAO
{
    public class ShoppingCartDAO
    {

        string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

        string getIdQry = "Select @@Identity";

        string insertOrderQry = "insert into [Orders] ([UserId], [Total], [Shipping], [Taxes]) values (@UserId, @Total, @Shipping, @Taxes)";

        string insertOrderItemsQry = "insert into [OrderItems]([ProductId], [Price],[Qty]) values (@ProductId, @Price, @Qty)";

        string insertOrderXOrderItemsQry = "insert into [Orders_X_OrderItems] ([OrderItemId], [OrderId]) values (@OrderItemId, @OrderId)";

        public int InsertOrder(ShoppingCart cart, SqlConnection conn, SqlTransaction tran)
        {
                using(SqlCommand cmd = new SqlCommand(insertOrderQry, conn))
                {
                    cmd.Transaction = tran;
                    cmd.Parameters.AddWithValue("@UserId", cart.CustomerId);
                    cmd.Parameters.AddWithValue("@Total", cart.getTotal());
                    cmd.Parameters.AddWithValue("@Shipping", cart.getShipping());
                    cmd.Parameters.AddWithValue("@Taxes", cart.getTaxes());

                    try
                    {
                        int orderid = 0;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = getIdQry;
                        orderid = Convert.ToInt32(cmd.ExecuteScalar());
                        return orderid;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                    }
                }
        }

        public int InsertOrderItems(ProductModel product, int OrderId, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand(insertOrderItemsQry, conn))
            {
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Qty", product.Quantity);

                try
                {
                    int orderitemid = 0;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = getIdQry;
                    orderitemid = Convert.ToInt32(cmd.ExecuteScalar());
                    return orderitemid;
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                }
            }
        }

        public bool RelateOrderAndItems(int OrderId, int OrderItemId, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand(insertOrderXOrderItemsQry, conn))
            {
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@OrderItemId", OrderId);
                cmd.Parameters.AddWithValue("@OrderId", OrderItemId);

                try
                {
                    
                    var results = cmd.ExecuteNonQuery();
                    if((int)results > 0)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                }
            }
        }

        public bool AddOrder(ShoppingCart cart)
        {
            SqlTransaction tran = null;
            using (SqlConnection conn = new SqlConnection(dbConnStr))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    int orderid = this.InsertOrder(cart, conn, tran);

                    if(orderid > 0)
                    {
                        foreach(ProductModel product in cart.products)
                        {
                            int orderitemid = this.InsertOrderItems(product, orderid, conn, tran);
                            if(orderitemid > 0)
                            {
                                bool insertSuccessful = this.RelateOrderAndItems(orderid, orderitemid,conn, tran);
                                if (!insertSuccessful)
                                {
                                    tran.Rollback();
                                    conn.Close();
                                    return false;
                                }
                            } else
                            {
                                tran.Rollback();
                                conn.Close();
                                return false;
                            }
                        }
                    } else
                    {
                        tran.Rollback();
                        conn.Close();
                        return false;
                    }
                    tran.Commit();
                    conn.Close();
                    return true;
                }
                catch (SqlException ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }
                    conn.Close();
                    throw new Exception("An error occured adding user to the users table.\nError: " + ex.Message);
                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }
                    conn.Close();
                    throw new Exception("An unexpected error has occured.\nError: " + ex.Message);
                }
            }
        }
    }
}