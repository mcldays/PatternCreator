using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class StampModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } 
        public byte[] Image { get; set; }
        public virtual ICollection<StampPositions> StampPositions { get; set; }

        public StampModel()
        {
            StampPositions = new List<StampPositions>();
        }
    }
}