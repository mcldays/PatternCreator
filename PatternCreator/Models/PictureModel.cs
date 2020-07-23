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
        public virtual ICollection<StampPositions> StampPositions { get; set; }
        public virtual ICollection<PositionModel> PositionModels { get; set; }

        public PictureModel()
        {
            PositionModels = new List<PositionModel>();
            StampPositions = new List<StampPositions>();
        } 
    }
    public class PictureModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public byte[] Image { get; set; }
        public List<StampPositionsViewModel> StampPositions { get; set; }
        public List<PositionModel> PositionModels { get; set; }

        public PictureModelViewModel(PictureModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Image = model.Image;
            PositionModels = model.PositionModels.ToList();
            StampPositions = model.StampPositions.Select(t=> new StampPositionsViewModel(t)).ToList();
        }
    }
}