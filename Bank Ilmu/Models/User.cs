using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bank_Ilmu.Models
{
    public class User
    {
        public string username { get; set; }
        public string email { get; set; }
        public string[,] posts { get; set; }
    }
}