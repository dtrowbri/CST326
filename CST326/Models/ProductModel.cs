using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST326.Models
{
    public class ProductModel
    {
        private ProductCategory category;

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }

        //public string Category { get; set; }

        public ProductCategory Category { get; set; }

        public string ProductImageLocation { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
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