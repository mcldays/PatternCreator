using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class SetBlockModel
    {
        public string picId { get; set; }
        public List<List<string>> bounds { get; set; }
    }
}