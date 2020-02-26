using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using PatternCreator.Utilities;

namespace PatternCreator.Controllers
{
    public class PrintController : Controller
    {
        [HttpPost]
        public string GetComputedPhotos(string data)
        {
            var photos = new List<string>();
            var users = SendDbUtility.GetAllUsers();
            var templates = SendDbUtility.GetAllTemplates();
            var positions = SendDbUtility.GetAllPositions();

            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(data);
            var usersFromCompanies = users.Where(t => parsed["companies"].Contains(t.CompanyId)).Select(t => t.Id);
            var substrate = parsed["substrate"][0] == 1;

            var neededUsers = parsed["users"].Concat(usersFromCompanies);
            foreach (var user in neededUsers)
            foreach (var template in parsed["templates"])
                photos.Add(Convert.ToBase64String(ComputePhoto.Compute(users.Find(t => t.Id == user),
                    templates.Find(t => t.Id == template), positions.Where(t => t.PictureId == template).ToList(), substrate)));

            return string.Join("<separator>", photos);
        }


      
    }
}