using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Ilmu.Models
{
    public class Post
    {
        public string postid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string likecount { get; set; }
        public string sharecount { get; set; }
        public string downloadcount { get; set; }
        public string timestamp { get; set; }
        public string isliked { get; set; }
        public List<List<string>> comments { get; set; }
    }
}