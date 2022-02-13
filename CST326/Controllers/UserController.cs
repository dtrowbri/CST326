using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CST326.Models;
using CST326.DAO;

namespace CST326.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(UserModel user)
        {
            UserDAO dao = new UserDAO();

            var results = dao.AddUser(user);

            return Content(results.ToString());
        } 

        [HttpPost]
        public ActionResult AuthenticateUser(UserModel user)
        {
            UserDAO dao = new UserDAO();

            var results = dao.Authenticate(user);

            return Content(results.ToString());
        }
    }
}