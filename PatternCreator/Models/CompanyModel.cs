using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class CompanyModel
    {
        [Key]
        public int Id { get; set; }

        public string CompanyName { get; set; }
    }
}