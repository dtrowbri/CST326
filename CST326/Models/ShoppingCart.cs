using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CST326.Models;

namespace CST326.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.products = new List<ProductModel>();
        }
        public List<ProductModel> products { get; set; }

        public double getTaxes()
        {
            return getSubTotal() * 0.07;
        }

        private double getSubTotal()
        {
            double subtotal = 0.0d;
            if(this.products.Count > 0)
            {
                foreach(ProductModel product in this.products)
                {
                    subtotal += product.Price;
                }
            }
            return subtotal;
        }

        public double getShipping()
        {
            return getSubTotal() * 0.1;
        }

        public double getTotal()
        {
            return getSubTotal() + getTaxes() + getShipping();
        }

        public void Add(ProductModel product)
        {
            this.products.Add(product);
        }

        public int CustomerId { get; set; }

        public int AddressId { get; set; }
        public int CreditCardId { get; set; }
    }
}