﻿using System;
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
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double Width { get; set; }

        public string Type { get; set; }
        public int FontSize { get; set; }
        public double Height { get; set; }
        public string Text { get; set; }
        [ForeignKey("PictureId")]
        public virtual PictureModel PictureModel { get; set; }
    }
}