using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace CST326.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            this.OrderItems = new List<ProductModel>();
        }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CreditCardId { get; set; }
        public double Total { get; set; }
        public double Tax { get; set; }
        public double AddressId { get; set; }
        public double Shipping { get; set; }
        public List<ProductModel> OrderItems { get; set; }
        public void AddOrderItem(ProductModel product)
        {
            OrderItems.Add(product);
        }
    }
}