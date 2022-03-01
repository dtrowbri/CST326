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
    public class StoreRepController : Controller
    {
        // GET: StoreRep
        public ActionResult ProductList()
        {
            ProductDAO dao = new ProductDAO();
            List<ProductModel> productlist = dao.GetAllProducts();
            return View(productlist);
        }

        [HttpGet]
        public ActionResult EditProduct(int ProductId = -1)
        {
            if (ProductId > 0)
            {
                ProductDAO dao = new ProductDAO();
                ProductModel product = dao.GetProduct(ProductId);

                return View(product);
            } else
            {
                return RedirectToAction("ProductList");
            }
        }

        [HttpPost]
        public ActionResult EditProduct(ProductModel product)
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
                bool wasSuccessful = dao.EditProduct(product);
                if (wasSuccessful)
                {
                    return Content("The product has been updated");
                } else
                {
                    return Content("Failed to update product. Please contact administrator.");
                }
            }
            catch (Exception Ex)
            {
                return Content(Ex.Message);
            }
        }


        [HttpGet]
        public ActionResult DeleteProduct(int ProductId)
        {
            try
            {
                ProductDAO dao = new ProductDAO();
                bool wasSuccessful = dao.DeleteProduct(ProductId);

                if (wasSuccessful)
                {
                    return Content("The product was deleted successfully");
                } else
                {
                    return Content("The product failed to delete. Please contact your administrator.");
                }
            }
            catch(Exception Ex)
            {
                return Content(Ex.Message);
            }
        }
    }
}