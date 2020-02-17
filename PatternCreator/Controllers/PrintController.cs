using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace PatternCreator.Controllers
{
    public class PrintController : Controller
    {
        [HttpPost]
        public byte[][] GetComputedPhotos(string data)
        {
            var photos = new List<byte[]>();
            var users = Utilities.SendDbUtility.GetAllUsers();
            var templates = Utilities.SendDbUtility.GetAllTemplates();
            var positions = Utilities.SendDbUtility.GetAllPositions();

            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(data);
            foreach (var user in parsed["users"])
            {
                foreach (var template in parsed["templates"])
                {
                    photos.Add(Utilities.ComputePhoto.Compute(users.Find(t=>t.Id == user), templates.Find(t=>t.Id == template), positions.Where(t => t.PictureId == template).ToList()));
                }
            }

            return photos.ToArray();
        }
    }
}