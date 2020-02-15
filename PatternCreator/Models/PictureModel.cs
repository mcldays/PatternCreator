using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class PictureModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // название картинки
        public byte[] Image { get; set; }
    }
}