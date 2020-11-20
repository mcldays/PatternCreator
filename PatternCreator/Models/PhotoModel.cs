using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class PhotoModel
    {
        [Key]
        public int PhotoModelId { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public int PicId { get; set; }
        [ForeignKey("PicId")]
        public virtual PictureModel PictureModel { get; set; }
    }
    public class PhotoViewModel
    {
        public int PhotoModelId { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int PicId { get; set; }

        public PhotoViewModel()
        {

        }
        public PhotoViewModel(PhotoModel model)
        {
            PhotoModelId = model.PhotoModelId;
            PosX = model.PosX    .ToString();
            PosY = model.PosY    .ToString();
            Width = model.Width  .ToString();
            Height = model.Height.ToString();
            PicId = model.PicId;
        }

    }
}