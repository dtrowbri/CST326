using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CST326.Models;
using CST326.DAO;
using System.IO;

namespace CST326.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult CreateProduct(ProductModel product)
        {
            if (product.ImageFile != null)
            {
                try
                {
                    string filename = Path.GetFileName(product.ImageFile.FileName);
                    filename = DateTime.Now.ToString("MMddyyyyHHmmss") + filename;
                    string serverpath = Path.Combine(Server.MapPath("~/ProductImages"), filename);
                    product.ImageFile.SaveAs(serverpath);
                    product.ProductImageLocation = "/ProductImages/" + filename;
                }
                catch
                {
                    return Content("Error uploading image");
                }
            }

            ProductDAO dao = new ProductDAO();
            try
            {
                dao.CreateProduct(product);
            } catch (Exception Ex)
            {
                return Content(Ex.Message);
            }

            return Content("Product added successfully");
        }

        public ActionResult StoreFront()
        {
            ProductDAO dao = new ProductDAO();
            List<ProductModel> productlist = dao.GetAllProducts();
            return View(productlist);
        }

        public ActionResult ViewProduct(int ProductId)
        {
            ProductDAO dao = new ProductDAO();
            ProductModel product = dao.GetProduct(ProductId);

            return View(product);
        }
    }
}