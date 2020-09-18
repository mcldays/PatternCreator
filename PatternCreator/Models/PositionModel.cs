using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class PositionModel
    {
        [Key]
        public int Id { get; set; }
        public int PictureId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Width { get; set; }

        public string Type { get; set; }
        public string FontWeight { get; set; }
        public string Alignment { get; set; }
        public int FontSize { get; set; }
        public float Height { get; set; }
        public string Text { get; set; }
        [ForeignKey("PictureId")]
        public virtual PictureModel PictureModel { get; set; }
    }
}