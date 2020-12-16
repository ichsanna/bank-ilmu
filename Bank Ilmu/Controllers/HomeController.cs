using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using Bank_Ilmu.Models;
using System.Configuration;

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
                List<List<string>> contents = new List<List<string>>();
                List<List<string>> comments = new List<List<string>>();
                List<string> likes = new List<string>();
                MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
                if (con.State == ConnectionState.Closed) con.Open();
                string strSelect = "SELECT * FROM contents";
                MySqlCommand cmd = new MySqlCommand(strSelect, con);
                MySqlDataReader myReader = cmd.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    contents.Add(new List<string>());
                    contents[count].Add(myReader["Id"].ToString());
                    contents[count].Add(myReader["title"].ToString());
                    contents[count].Add(myReader["description"].ToString());
                    contents[count].Add(myReader["owner"].ToString());
                    contents[count].Add(myReader["likecount"].ToString());
                    contents[count].Add(myReader["sharecount"].ToString());
                    contents[count].Add(myReader["downloadcount"].ToString());
                    contents[count].Add(myReader["link"].ToString());
                    contents[count].Add(myReader["timestamp"].ToString());
                    count++;
                }
                myReader.Close();
                string strSelect2 = "SELECT * FROM comments";
                MySqlCommand cmd2 = new MySqlCommand(strSelect2, con);
                MySqlDataReader myReader2 = cmd2.ExecuteReader();
                count = 0;
                while (myReader2.Read())
                {
                    comments.Add(new List<string>());
                    comments[count].Add(myReader2["user"].ToString());
                    comments[count].Add(myReader2["comment"].ToString());
                    comments[count].Add(myReader2["contentid"].ToString());
                    comments[count].Add(myReader2["timestamp"].ToString());
                    count++;
                }
                myReader2.Close();
                string strSelect3 = "SELECT * FROM likes WHERE user = '" + Session["username"] + "'";
                MySqlCommand cmd3 = new MySqlCommand(strSelect3, con);
                MySqlDataReader myReader3 = cmd3.ExecuteReader();
                while (myReader3.Read())
                {
                    likes.Add(myReader["contentid"].ToString());
                }
                myReader3.Close();
                con.Close();
                //return Content(contents.Count.ToString() + comments.Count.ToString());
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
                        link = contents[i][7],
                        timestamp = contents[i][8],
                        isliked = "false"
                    });
                        for (int j = 0; j < comments.Count; j++)
                        {
                            if (comments[j][2] == contents[i][0])
                            {
                                data[i].comments = new List<List<String>>()
                            {
                                new List<string>() {comments[i][0],comments[i][1],comments[i][3]}
                            };
                            }
                        }
                    for (int k = 0; k < likes.Count; k++)
                    {
                        if (likes[k] == contents[i][0])
                        {
                            data[i].isliked = "true";
                        }
                    }
                }
                if (data != null) ViewBag.data = data;
                return View();
            }
        }
    }
}