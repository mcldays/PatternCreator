using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;
using WebGrease.Css.Extensions;

namespace PatternCreator.Controllers
{
    public class PrintController : Controller
    {
        [HttpPost]
        public string GetComputedPhotos(string data)
        {
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(data);
            using (UserContext db = new UserContext())
            {
                List<UserModel> userModels = new List<UserModel>();
                foreach (var ids in parsed["companies"])
                {
                    var company = db.CompanyModels.Find(ids);
                    if(company==null)
                        continue;
                    company.UserModels.ForEach(t=> userModels.Add(t));
                }
                foreach (var ids in parsed["users"])
                {
                    if (userModels.Any(t=>t.Id==ids))
                        continue;
                    var user = db.UserModels.Find(ids);
                    if (user == null)
                        continue;
                    userModels.Add(user);
                }
                var photos = new List<string>();
                

                List<PictureModel> templates = new List<PictureModel>();
                foreach (var ids in parsed["templates"])
                {
                    var template = db.PicturesModels.Find(ids);
                    if (template == null)
                        continue;
                    templates.Add(template);
                }
               
                var substrate = parsed["substrate"][0] == 1;

                foreach (var user in userModels)
                foreach (var template in templates)
                    photos.Add(Convert.ToBase64String(ComputePhoto.Compute(user,
                        template,
                        substrate)));

                return string.Join("<separator>", photos);
            }
        }
    }
}