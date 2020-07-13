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
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }


    }
    public class AutModelViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


    }
}