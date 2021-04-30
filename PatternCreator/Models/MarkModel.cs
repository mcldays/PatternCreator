using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatternCreator.Models
{
    public class MarkModel
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public string Naming { get; set; }
        public int HoursNumber { get; set; }
        public int? SpecialtyId { get; set; }
        [ForeignKey("SpecialtyId")]
        public virtual SpecialtyModel SpecialtyModel { get; set; }
    }
    public class MarkViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        
        public int? Number { get; set; }
        [Required(ErrorMessage = "Наименование не должно быть пустым")]
        public string Naming { get; set; }
        [Required(ErrorMessage = "Введите кол-во часов")]
        public int HoursNumber { get; set; }
        [HiddenInput]
        public int? SpecialtyId { get; set; }

        public MarkViewModel(MarkModel model)
        {
            Id = model.Id;
            Number = model.Number;
            Naming = model.Naming;
            HoursNumber = model.HoursNumber;
            SpecialtyId = model.SpecialtyId;
        }
        public MarkViewModel()
        {

        }
       
    }
}