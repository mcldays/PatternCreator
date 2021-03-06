﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class SetBlockModel
    {
        public string picId { get; set; }
        public List<List<string>> bounds { get; set; }
        public List<StampConvertModel> stamps { get; set; }
        public List<PhotoViewModel> photos { get; set; }
        public List<StaticImageViewModel> images { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string NaturalWidth { get; set; }
        public string NaturalHeight { get; set; }
    }
}