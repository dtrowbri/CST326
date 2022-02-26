using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CST326.Models;
using CST326.DAO;

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
    }
}