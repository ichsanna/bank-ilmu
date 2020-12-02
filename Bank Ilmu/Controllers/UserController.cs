using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

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
            if (Session["username"] == null) return View();
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            if (Session["username"] == null) return View();
            else return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult LoginSubmit(string username, string password)
        {
            string response = "Incorrect username or password";
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                string strSelect = "SELECT * FROM users WHERE username = '"+username+ "' AND password = '" + password + "'";
                SqlCommand cmd = new SqlCommand(strSelect, con);
                SqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    System.Diagnostics.Debug.WriteLine(myReader["username"]+"\n");
                    if (myReader["username"].ToString()==username && myReader["password"].ToString() == password)
                    {
                        Session["username"] = username;
                        response = "Successfully logged in";
                    }
                }
                myReader.Close();
                con.Close();
                ViewBag.response = response;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.response = "Please fill the login credentials";
                return RedirectToAction("Login", "User");
            }
                
        }
        [HttpPost]
        public ActionResult RegisterSubmit(string email, string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(email))
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO users (email,username,password) VALUES ('"+email+"','"+username+"','"+password+"')";
                cmd.ExecuteNonQuery();
                con.Close();
                Session["username"] = username;
                ViewBag.response = "Successfully registered and logged in";
                return RedirectToAction("Register", "User");
            }
            else
            {
                ViewBag.response = "Please fill the required information";
                return RedirectToAction("Register", "User");
            }
        }
    }
}