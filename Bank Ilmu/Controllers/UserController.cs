using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank_Ilmu.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public string LoginSubmit(string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                //TODO: Get the data from database
                Session["username"] = username;
                return "Successfully logged in";
            }
            else
                return "Please fill the login credentials";
        }
        public string RegisterSubmit(string email, string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(email))
            {
                //TODO: Post the data from database
                Session["username"] = username;
                return "Successfully registered and logged in";
            }
            else
                return "Please fill the required information";
        }
    }
}