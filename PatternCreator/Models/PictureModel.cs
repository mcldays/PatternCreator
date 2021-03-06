﻿using System;
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
        //public byte[] Image { get; set; }
        public string PathToImg { get; set; }
        public int NaturalWidth { get; set; }
        public int NaturalHeight { get; set; }
        public int BlankNumber { get; set; }
        public virtual ICollection<DocumentModel> DocumentModels { get; set; }
        public virtual ICollection<StampPositions> StampPositions { get; set; }
        public virtual ICollection<PhotoModel> PhotoModels { get; set; }
        public virtual ICollection<StaticImageModel> StaticImageModels { get; set; }

        public virtual ICollection<PositionModel> PositionModels { get; set; }
        public virtual ICollection<SpecialtyModel> Specialties { get; set; }
        public PictureModel()
        {
            DocumentModels = new List<DocumentModel>();
            PositionModels = new List<PositionModel>();
            StampPositions = new List<StampPositions>();
            StaticImageModels = new List<StaticImageModel>();
            PhotoModels = new List<PhotoModel>();
            Specialties = new List<SpecialtyModel>();
        } 
    }
    public class PictureModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToImg { get; set; }

        public List<DocumentModel> DocumentModels { get; set; }
        public List<StampPositionsViewModel> StampPositions { get; set; }
        public List<PhotoViewModel> PhotoModels { get; set; }
        public List<StaticImageViewModel> StaticImageModels { get; set; }

        public List<PositionModel> PositionModels { get; set; }
        public int NaturalWidth { get; set; }
        public int NaturalHeight { get; set; }
        public PictureModelViewModel(PictureModel model)
        {
            Id = model.Id;
            Name = model.Name;
            PathToImg = model.PathToImg;
            NaturalWidth = model.NaturalWidth;
            NaturalHeight = model.NaturalHeight;
            PositionModels = model.PositionModels.ToList();
            PhotoModels = model.PhotoModels.Select(t => new PhotoViewModel(t)).ToList();
            StampPositions = model.StampPositions.Select(t=> new StampPositionsViewModel(t)).ToList();
            StaticImageModels = model.StaticImageModels.Select(t => new StaticImageViewModel(t)).ToList();
        }
    }
}