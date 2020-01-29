using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class AutModel
    {
        [Key]
        public int id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }


    }
}