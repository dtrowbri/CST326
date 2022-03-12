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
                    TempData["Error"] = "Error uploading image";
                    return View("AddProduct");
                }
            }

            ProductDAO dao = new ProductDAO();
            try
            {
                dao.CreateProduct(product);
            } catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message;
                return View("AddProduct");
            }
            TempData["Success"] = "Product added successfully";
            return View("ProductList");
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

        [HttpPost]
        public ActionResult AddToCart(ProductModel product)
        {
            int quantity = product.Quantity;
            ProductDAO dao = new ProductDAO();
            product = dao.GetProduct(product.ProductId);
            product.Quantity = quantity;
            if (quantity <= 0)
            {
                TempData["isAdded"] = "Quantity must be at least one";
                return View("ViewProduct", product);
            }
           
            ShoppingCart shoppingCart;
            if(Session["ShoppingCart"] != null)
            {
                shoppingCart = (ShoppingCart)Session["ShoppingCart"];
            } else
            {
                shoppingCart = new ShoppingCart();
                if (Session["User"] != null)
                {
                    shoppingCart.CustomerId = (int)(((UserModel)Session["User"]).UserId);
                } else
                {
                    shoppingCart.CustomerId = 1;
                }
            }

            shoppingCart.Add(product);

            Session["ShoppingCart"] = shoppingCart;
            TempData["isAdded"] = "Added to Cart!";
            return View("ViewProduct", product);
        }
    }
}