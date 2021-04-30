using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PatternCreator.Controllers;

namespace PatternCreator.Models
{
    public class StampPositions
    {
        [Key]
        public int StampPositionId { get; set; }
        
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public PatternController.StampType Type { get; set; }

        public int PicId { get; set; }
        [ForeignKey("PicId")]
        public virtual PictureModel PictureModel { get; set; }
        

    }
    
    public class StampPositionsViewModel
    {
        public static string GetTextByType(PatternController.StampType type)
        {
            switch (type)
            {
                case PatternController.StampType.Stamp: return "Штамп";
                case PatternController.StampType.СhairmanSignature: return "Подпись председателя";
                case PatternController.StampType.TeacherSignature: return "Подпись преподавателя";
                case PatternController.StampType.SecretarySignature: return "Подпись секретаря";
                default: return string.Empty;
            }
        }

        public int StampPositionId { get; set; }

        public float PosX   { get; set; }
        public float PosY   { get; set; }
        public float Width  { get; set; }
        public float Height { get; set; }
        public PatternController.StampType Type { get; set; }

        public int StampId { get; set; }
        public string Text { get; set; }


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
            PicId = model.PicId;
            Type = model.Type;
            Text = GetTextByType(model.Type);
        }

    }

    public class StampConvertModel
    {
        public int StampPositionId { get; set; }

        public string PosX { get; set; }
        public string PosY { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public PatternController.StampType Type { get; set; }
        public int PicId { get; set; }
        public StampConvertModel()
        {

        }
    }

}