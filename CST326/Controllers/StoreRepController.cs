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

        public ActionResult Login()
        {
            return View();       
        }

        public ActionResult NewEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeModel employee)
        {
            ModelState["FirstName"].Errors.Clear();
            if (employee.FirstName == null || employee.LastName == null || employee.Email == null || employee.PhoneNumber == null || employee.Password == null)
            {
                ModelState.AddModelError("FirstName", "All fields must be populated before creating your account.");
                return View("NewEmployee", employee);
            }

            EmployeeDAO dao = new EmployeeDAO();

            var results = dao.AddEmployee(employee);

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult AuthenticateEmployee(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                EmployeeDAO dao = new EmployeeDAO();

                var emp = dao.Authenticate(employee);

                if (emp.Email != null)
                {
                    Session["Employee"] = emp;
                    return RedirectToAction("ProductList", "StoreRep");
                }
                else
                {
                    ModelState.AddModelError("EmployeeId", "Employee ID or Password is incorrect. Please try again.");
                    return View("Login", employee);

                }
            }
            else
            {
                return View("Login", employee) ;
            }
        }

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
                    // return Content("The product has been updated");
                    return View("ViewProduct", product);
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
                    TempData["DeletionStatus"] = "Product Successfully Deleted!";
                    return View("ProductList");
                } else
                {

                    TempData["DeletionStatus"] = "The product failed to delete. Please contact your administrator.";
                    return View("ProductList");
                }
            }
            catch(Exception Ex)
            {
                return Content(Ex.Message);
            }
        }

        public ActionResult SignOut()
        {
            if (Session["Employee"] != null)
            {
                Session["Employee"] = null;
            }
            return RedirectToAction("Login");
        }
    }
}