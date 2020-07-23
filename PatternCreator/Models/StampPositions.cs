using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class StampPositions
    {
        [Key]
        public int StampPositionId { get; set; }
        
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public int StampId { get; set; }
        [ForeignKey("StampId")]
        public virtual StampModel StampModel { get; set; }

        public int PicId { get; set; }
        [ForeignKey("PicId")]
        public virtual PictureModel PictureModel { get; set; }
        

    }
    public class StampPositionsViewModel
    {
        
        public int StampPositionId { get; set; }

        public double PosX   { get; set; }
        public double PosY   { get; set; }
        public double Width  { get; set; }
        public double Height { get; set; }

        public int StampId { get; set; }
        
        public byte[] Image { get; set; }

        public int PicId { get; set; }

        public StampPositionsViewModel()
        {

        }
        public StampPositionsViewModel(StampPositions model)
        {
            StampPositionId = model.StampPositionId;
            PosX = model.PosX;
            PosY  = model.PosY  ;
            Width = model.Width ;
            Height= model.Height;
            StampId = model.StampId;
            Image = model.StampModel.Image;
            PicId = model.PicId;
        }

    }

    public class StampConvertModel
    {
        public int StampPositionId { get; set; }

        public string PosX { get; set; }
        public string PosY { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public int StampId { get; set; }

        public int PicId { get; set; }
        public StampConvertModel()
        {

        }
    }

}