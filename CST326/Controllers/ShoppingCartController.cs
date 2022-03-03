using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using CST326.Models;
using CST326.DAO;

namespace CST326.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult ShoppingCart()
        {
            ShoppingCart shoppingCart;
            if (Session["ShoppingCart"] != null)
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
                Session["ShoppingCart"] = shoppingCart;
            }

            return View(shoppingCart);
        }

        [HttpPost]
        public ActionResult Createorder()
        {
            ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
            ShoppingCartDAO dao = new ShoppingCartDAO();
            bool wasSuccessful = dao.AddOrder(cart);
            if (wasSuccessful)
            {
                Session["ShoppingCart"] = null;
                return Content("Order submitted");
            } else
            {
                return Content("Order was not submitted");
            }
        }
    }
}