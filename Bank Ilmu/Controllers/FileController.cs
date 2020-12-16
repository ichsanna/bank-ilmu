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

namespace Bank_Ilmu.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Upload(string title, string description, IFormFile materi)
        {
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(description))
            {
                System.Diagnostics.Debug.WriteLine(title + description + "\n");
                int contentid = 0;
                if (materi == null || materi.Length == 0)
                    return Content("file not selected");
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO contents (title,description,owner,timestamp) VALUES ('" + title + "','" + description + "','" + Session["username"] + "','" + DateTime.Now.ToString("dddd, dd MMMM yyyy") + "')";
                cmd.ExecuteNonQuery();
                string strSelect = "SELECT * FROM contents WHERE title = '" + title + "'";
                SqlCommand cmd1 = new SqlCommand(strSelect, con);
                SqlDataReader myReader = cmd1.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["title"].ToString() == title)
                    {
                        contentid = Convert.ToInt32(myReader["Id"]) + 1;
                    }
                }
                myReader.Close();
                con.Close();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "file"+contentid);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await materi.CopyToAsync(stream);
                }
                ViewBag.response = "Successfully uploaded";
                return RedirectToAction("Home", "Index");
            }
            else
            {
                ViewBag.response = "Please fill the required information";
                return RedirectToAction("", "User");
            }
        }
        public async Task<ActionResult> Download(string contentid)
        {
            string filename = "file" + contentid;
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "Files", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            int dlcount = 0;
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bankilmu.mdf;Integrated Security=True");
            if (con.State == ConnectionState.Closed) con.Open();
            string strSelect = "SELECT * FROM contents WHERE Id = '" + contentid + "'";
            SqlCommand cmd = new SqlCommand(strSelect, con);
            SqlDataReader myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader["contentid"].ToString() == contentid)
                {
                    dlcount = Convert.ToInt32(myReader["sharecount"]) + 1;
                }
            }
            myReader.Close();
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "UPDATE contents SET downloadcount = " + dlcount + " WHERE Id = '" + contentid + "'";
            cmd2.ExecuteNonQuery();
            con.Close();
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}