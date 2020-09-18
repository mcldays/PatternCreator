using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatternCreator.Models
{
    public class SpecialtyModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SpecialityName { get; set; }
        public string FieldSpecialty { get; set; }
        public string Quality { get; set; }

        public string Prefix { get; set; }
        public virtual ICollection<PictureModel> Pictures { get; set; }
        //public virtual ICollection<ProtocolModel> Protocols { get; set; }

        public SpecialtyModel()
        {
            //Protocols = new List<ProtocolModel>();
            Pictures = new List<PictureModel>();
        }


    }
    public class SpecialtyViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote(action:"CheckSpeciality",controller:"Pattern", ErrorMessage = "Данная специальность уже существует")]
        public string SpecialityName { get; set; }
        public string Prefix { get; set; }
        public string FieldSpecialty { get; set; }
        public string Quality { get; set; }
        public List<int> Templates { get; set; }
        public SpecialtyViewModel(SpecialtyModel model)
        {
            Id = model.Id;
            SpecialityName = model.SpecialityName;
            Prefix = model.Prefix;
            Quality = model.Quality;
            FieldSpecialty = model.FieldSpecialty;
            Templates = new List<int>(model.Pictures.Select(t=>t.Id));
        }
        public SpecialtyViewModel()
        {
            Id = 0;
        }


    }
}