using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Bank_Ilmu.Models;

namespace Bank_Ilmu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                List<Post> data = new List<Post>();
                List<List<String>> contents = new List<List<String>>();
                List<List<String>> comments = new List<List<String>>();
                List<String> likes = new List<String>();
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                string strSelect = "SELECT * FROM contents";
                SqlCommand cmd = new SqlCommand(strSelect, con);
                SqlDataReader myReader = cmd.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    contents[count].Add(myReader["Id"].ToString());
                    contents[count].Add(myReader["title"].ToString());
                    contents[count].Add(myReader["description"].ToString());
                    contents[count].Add(myReader["owner"].ToString());
                    contents[count].Add(myReader["likecount"].ToString());
                    contents[count].Add(myReader["sharecount"].ToString());
                    contents[count].Add(myReader["downloadcount"].ToString());
                    contents[count].Add(myReader["timestamp"].ToString());
                    count++;
                }
                myReader.Close();
                string strSelect2 = "SELECT * FROM comments";
                SqlCommand cmd2 = new SqlCommand(strSelect2, con);
                SqlDataReader myReader2 = cmd2.ExecuteReader();
                count = 0;
                while (myReader2.Read())
                {
                    comments[count].Add(myReader2["user"].ToString());
                    comments[count].Add(myReader2["comment"].ToString());
                    comments[count].Add(myReader2["contentid"].ToString());
                    comments[count].Add(myReader2["timestamp"].ToString());
                    count++;
                }
                myReader2.Close();
                string strSelect3 = "SELECT * FROM likes WHERE username = '" + Session["username"] + "'";
                SqlCommand cmd3 = new SqlCommand(strSelect3, con);
                SqlDataReader myReader3 = cmd3.ExecuteReader();
                while (myReader3.Read())
                {
                    likes.Add(myReader["contentid"].ToString());
                }
                myReader3.Close();
                con.Close();
                for (int i = 0; i < contents.Count; i++)
                {
                    data.Add(new Post()
                    {
                        postid = contents[i][0],
                        title = contents[i][1],
                        description = contents[i][2],
                        author = contents[i][3],
                        likecount = contents[i][4],
                        sharecount = contents[i][5],
                        downloadcount = contents[i][6],
                        timestamp = contents[i][7],
                        isliked = "false"
                    });
                    for (int j = 0; j < comments.Count; j++)
                    {
                        if (comments[j][2] == contents[i][0])
                        {
                            data[0].comments = new List<List<String>>()
                            {
                                new List<string>() {comments[i][0],comments[i][1],comments[i][3]}
                            };
                        }
                    }
                    for (int k = 0; k < likes.Count; k++)
                    {
                        if (likes[k] == contents[i][0])
                        {
                            data[0].isliked = "true";
                        }
                    }
                }
                ViewBag.data = data;
                return View();
            }
        }

        public ActionResult Addnew()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}