using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class AutoTextModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Text { get; set; }
    }
}