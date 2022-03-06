using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using CST326.Models;

namespace CST326.DAO
{
    public class OrdersDAO
    {
		string dbConnStr = ConfigurationManager.ConnectionStrings["SiteDB"].ConnectionString;

		string getOrderItemsQry = @"
				select
					orders.[OrderId] as 'OrderId'
					, orders.[UserId] as 'UserId'
					, orders.[CreditCardId] as 'CreditCardId'
					, orders.[Total] as 'Total'
					, orders.[Taxes] as 'Taxes'
					, orders.[Shipping] as 'Shipping'
					, orders.[AddressId] as 'AddressId'
					, products.[ProductId] as 'ProductId'
					, products.[Name] as 'ProductName'
					, products.[Description] as 'ProductDescription'
					, products.[ImageLocation] as 'ImageLocation'
					, oi.[Price] as 'ProductPrice'
					, oi.[Qty] as 'Quantity'
				from[Orders] orders
				join[Orders_X_OrderItems] rel
					on rel.OrderId = orders.OrderId
				join[OrderItems] oi
					on oi.ProductId = rel.OrderItemId
				join[Products] products
					on products.productId = oi.productid
				where orders.[OrderId] = @OrderId";

		string getCustomerOrdersQry = @"select [OrderId] as 'OrderId', [UserId] as 'UserId', [CreditCardId] as 'CreditCardId', [Total] as 'Total', 
									[Shipping] as 'Shipping', [Taxes] as 'Taxes', [AddressId] as 'AddressId' from [Orders] where [UserId] = @UserId";

		public List<OrderModel> getCustomerOrders(UserModel user)
        {
			using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
				using(SqlCommand cmd = new SqlCommand(getCustomerOrdersQry, conn))
                {
					cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    try
                    {
						conn.Open();
						SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
							List<OrderModel> orders = new List<OrderModel>();
                            while (reader.Read())
                            {
								OrderModel order = new OrderModel();
								//order.AddressId = (int)reader["AddressId"];
								//order.CreditCardId = (int)reader["CreditCardId"];
								order.OrderId = (int)reader["OrderId"];
								order.Tax = Convert.ToDouble(reader["Taxes"]);
								order.Total = Convert.ToDouble(reader["Total"]);
								order.UserId = (int)reader["UserId"];
								orders.Add(order);
                            }

							conn.Close();
							return orders;
                        }
                        else {
							conn.Close();
							return new List<OrderModel>();
						}

                    }
                    catch (SqlException ex)
                    {
						conn.Close();
						throw new Exception("There was an error retrieving the orders from the database for user {" + user.UserId.ToString() + "}. Ex: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
						conn.Close();
						throw new Exception("There was an unexpected error while trying to retrieve orders from the database for user {" + user.UserId.ToString() + "}. Ex: " + ex.Message);
                    }
                }
            }
        }
	
		public OrderModel getOrderDetails(int orderid)
        {
			using(SqlConnection conn = new SqlConnection(dbConnStr))
            {
				using(SqlCommand cmd = new SqlCommand(getOrderItemsQry, conn))
                {
					cmd.Parameters.AddWithValue("@OrderId", orderid);
                    try
                    {
						conn.Open();
						SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
							OrderModel order = new OrderModel();
							bool firstRow = true;
                            while (reader.Read())
                            {
								if (firstRow)
								{
									firstRow = false;
									order.OrderId = (int)reader["OrderId"];
									order.UserId = (int)reader["UserId"];
									order.Total = Convert.ToDouble(reader["Total"]);
									order.Tax = Convert.ToDouble(reader["Taxes"]);
									order.Shipping = Convert.ToDouble(reader["Shipping"]);
								}
								ProductModel product = new ProductModel();
								product.ProductId = (int)reader["ProductId"];
								product.Name = reader["ProductName"].ToString();
								product.Description = reader["ProductDescription"].ToString();
								product.ProductImageLocation = reader["ImageLocation"].ToString();
								product.Price = Convert.ToDouble(reader["ProductPrice"]);
								product.Quantity = (int)reader["Quantity"];
								order.AddOrderItem(product);
                            }
							conn.Close();
							return order;
                        }else
                        {
							conn.Close();
							return new OrderModel();
                        }
                    }catch(SqlException ex)
                    {
						conn.Close();
						throw new Exception("There was an error retrieving order {" + orderid.ToString() + " from the database. Ex: " + ex.Message);
                    }catch(Exception ex)
                    {
						conn.Close();
						throw new Exception("There was an unexpected error that occured while retrieving order {" + orderid.ToString() + " from the database. Ex: " + ex.Message);
                    }
                }
            }
        }
	}
}