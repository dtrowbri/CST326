using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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


        [Range(1, 1000)]
        [Display(Name= "QTY")]
        public int Quantity { get; set; }
    }

    public enum ProductCategory
    {
        RAM,
        Motherboard,
        Disk,
        Cables,
        Screen,
        PCI_Card,
        CPU,
        NIC,
        Keyboard,
        Mouse,
        Monitor,
        Power_Supply
    }
}