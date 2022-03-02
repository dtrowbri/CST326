using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CST326.Models;

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
            }

            return View(shoppingCart);
        }
    }
}