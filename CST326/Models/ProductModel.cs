using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST326.Models
{
    public class ProductModel
    {
       // private ProductCategory category;

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }

        //public string Category { get; set; }

        public ProductCategory Category { get; set; }

        public string ProductImageLocation { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public ProductModel(int productId, string name, string description, double price, ProductCategory category, string productImageLocation, HttpPostedFileBase imageFile)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            ProductImageLocation = productImageLocation;
            ImageFile = imageFile;
        }

        public ProductModel()
        {
        }
    }

    public enum ProductCategory
    {
        RAM,
        Motherboard,
        Disk,
        Cables,
        Screen,
        PCI_Card,
        NIC,
        Keyboard,
        Mouse,
        Monitor,
        Power_Supply
    }
}