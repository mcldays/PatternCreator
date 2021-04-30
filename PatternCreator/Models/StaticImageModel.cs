using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class StaticImageModel
    {
        [Key]
        public int StaticImageModelId { get; set; }
        public string PathToImg { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int PicId { get; set; }
        [ForeignKey("PicId")]
        public virtual PictureModel PictureModel { get; set; }


    }
    public class StaticImageViewModel
    {
        public int StaticImageModelId { get; set; }
        public string PathToImg { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int PicId { get; set; }

        public StaticImageViewModel()
        {

        }
        public StaticImageViewModel(StaticImageModel model)
        {
            StaticImageModelId = model.StaticImageModelId;
            PathToImg = model.PathToImg;
            PosX = model.PosX.ToString();
            PosY = model.PosY.ToString();
            Width = model.Width.ToString();
            Height = model.Height.ToString();
            PicId = model.PicId;
        }

    }
}