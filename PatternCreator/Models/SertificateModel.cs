using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class SertificateModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserModel UserModel { get; set; }

        public int PictureId { get; set; }
        [ForeignKey("PictureId")]
        public virtual PictureModel PictureModel { get; set; }
    }
}