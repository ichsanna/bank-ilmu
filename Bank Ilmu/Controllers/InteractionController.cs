using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Bank_Ilmu.Controllers
{
    public class InteractionController : Controller
    {
        [HttpPost]
        public ActionResult Like(string contentid)
        {
            string owner = "a";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (con.State == ConnectionState.Closed) con.Open();
            string strSelect = "SELECT * FROM contents WHERE Id = '" + contentid + "'";
            SqlCommand cmd = new SqlCommand(strSelect, con);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader["contentid"].ToString() == contentid)
                {
                    owner = myReader["owner"].ToString();
                }
            }
            myReader.Close();
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "UPDATE contents SET likecount = likecount + 1 WHERE Id = '" + contentid + "'";
            cmd2.ExecuteNonQuery();
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "UPDATE users SET points = points + 1 WHERE username = '" + owner + "'";
            cmd1.ExecuteNonQuery();
            SqlCommand cmd3 = con.CreateCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "INSERT INTO likes (user,contentid) VALUES ('" + Session["username"] + "','" + contentid + "')";
            cmd3.ExecuteNonQuery();
            con.Close();
            return Content("");
        }
        [HttpPost]
        public ActionResult Share(string contentid)
        {
            string owner = "a";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (con.State == ConnectionState.Closed) con.Open();
            string strSelect = "SELECT * FROM contents WHERE Id = '" + contentid + "'";
            SqlCommand cmd = new SqlCommand(strSelect, con);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader["contentid"].ToString() == contentid)
                {
                    owner = myReader["owner"].ToString();
                }
            }
            myReader.Close();
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "UPDATE users SET points = points + 1 WHERE username = '" + owner + "'";
            cmd1.ExecuteNonQuery();
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "UPDATE contents SET sharecount = sharecount + 1 WHERE Id = '" + contentid + "'";
            cmd2.ExecuteNonQuery();
            con.Close();
            return Content("");
        }
        [HttpPost]
        public ActionResult Comment(string contentid,string comment)
        {
            string owner = "a";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (con.State == ConnectionState.Closed) con.Open();
            string strSelect = "SELECT * FROM contents WHERE Id = '" + contentid + "'";
            SqlCommand cmd = new SqlCommand(strSelect, con);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader["contentid"].ToString() == contentid)
                {
                    owner = myReader["owner"].ToString();
                }
            }
            myReader.Close();
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "UPDATE users SET points = points + 3 WHERE username = '" + owner + "'";
            cmd1.ExecuteNonQuery();
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "INSERT INTO comments (timestamp,user,comment,contentid) VALUES ('" + DateTime.Now.ToString("dddd, dd MMMM yyyy") + "','" + Session["username"] + "','" + comment + "','" + contentid + "')";
            cmd2.ExecuteNonQuery();
            con.Close();
            return Content("");
        }
    }
}