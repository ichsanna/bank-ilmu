using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Bank_Ilmu.Models;

namespace Bank_Ilmu.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("Login", "User");
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
            return RedirectToAction("Index", "User");
        }
        [HttpPost]
        public ActionResult LoginSubmit(UserLogin user)
        {
            string response = "Incorrect username or password";
            bool success = false;
            if (ModelState.IsValid)
            {
                user.password = CreateMD5(user.password);
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                string strSelect = "SELECT * FROM users WHERE username = '"+user.username+ "' AND password = '" + user.password + "'";
                SqlCommand cmd = new SqlCommand(strSelect, con);
                SqlDataReader myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["username"].ToString()==user.username && myReader["password"].ToString() == user.password)
                    {
                        Session["username"] = user.username;
                        Session["points"] = myReader["points"].ToString();
                        response = "Successfully logged in";
                        success = true;
                    }
                    else
                    {
                        success = false;
                        response = "Username atau password salah";
                    }
                }
                myReader.Close();
                con.Close();
                if (success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.response = response;
                    return View("Login");
                }
            }
            else
            {
                ViewBag.response = "Password atau Username tidak valid";
                return View("Login");
            }
                
        }
        [HttpPost]
        public ActionResult RegisterSubmit(User user)
        {
            if (ModelState.IsValid)
            {
                if(user.password != user.rePassword)
                {
                    ViewBag.response = "Password tidak sesuai";
                    return View("Register");
                }

                user.password = CreateMD5(user.password);
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO users (email,username,password) VALUES ('"+user.email+"','"+user.username+"','"+user.password+"')";
                cmd.ExecuteNonQuery();
                con.Close();
                Session["username"] = user.username;
                Session["points"] = 0;
                ViewBag.response = "Successfully registered and logged in";
                return RedirectToAction("Register", "User");
            }
            else
            {
                ViewBag.response = "Data tidak valid";
                return View("Register");
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}