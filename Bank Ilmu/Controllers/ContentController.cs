using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Bank_Ilmu.Controllers
{
    public class ContentController : Controller
    {
        [HttpPost]
        public ActionResult Post(string title, string description, string link)
        {
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(description))
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO contents (title,description,owner,link,timestamp) VALUES ('" + title + "','" + description + "','" + Session["username"] + "','" + link + "','" + DateTime.Now.ToString("dddd, dd MMMM yyyy") + "')";
                cmd.ExecuteNonQuery();
                con.Close();
                ViewBag.response = "Successfully posted";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.response = "Please fill the required information";
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Click(string link)
        {
            int dlcount = 0;
            string contentid = "a";
            string owner = "a";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (con.State == ConnectionState.Closed) con.Open();
            string strSelect = "SELECT * FROM contents WHERE link = '" + link + "'";
            SqlCommand cmd = new SqlCommand(strSelect, con);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                dlcount = Convert.ToInt32(myReader["downloadcount"]) + 1;
                contentid = myReader["Id"].ToString();
                owner = myReader["owner"].ToString();
            }
            myReader.Close();
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "UPDATE contents SET downloadcount = " + dlcount + " WHERE Id = '" + contentid + "'";
            cmd2.ExecuteNonQuery();
            SqlCommand cmd3 = con.CreateCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "UPDATE users SET points = points + 1 WHERE username = '" + owner + "'";
            cmd3.ExecuteNonQuery();
            con.Close();
            return Redirect(link);
        }
    }
}