using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;
using WebGrease.Css.Extensions;

namespace PatternCreator.Controllers
{
    public class PrintController : Controller
    {
        
        public string GetComputedPhotos(string data)
        {
            var document = JsonConvert.DeserializeObject<DocumentSaveModel[]>(data);
            bool substrate = (bool)TempData["Substrate"];
            using (UserContext db = new UserContext())
            {
                
                var photos = new List<string>();
                foreach (var model in document)
                {
                    DocumentModel doc = db.DocumentModels.Find(model.DocumentId);
                    if (doc != null)
                    {
                        photos.Add(Convert.ToBase64String(ComputePhoto.Compute(doc,
                            substrate)));
                    }
                        
                }
                

               
                    

                return string.Join("<separator>", photos);
            }
        }
        [HttpPost]
        public ActionResult SaveDocuments(DocumentSaveModel[] document)
        {
           
            using (UserContext db = new UserContext())
            {
                foreach (var model in document)
                {
                    DocumentModel document1 = db.DocumentModels.Find(model.DocumentId);
                    document1.Date = DateTime.Parse(model.Date);
                    db.Entry(document1).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }

            }

            string data1 = JsonConvert.SerializeObject(document);


            return RedirectToAction("GetComputedPhotos", new { data = data1});
        }
        public ActionResult GetDocuments(string data)
        {
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(data);
            using (UserContext db = new UserContext())
            {
                List<UserModel> userModels = new List<UserModel>();
                foreach (var ids in parsed["companies"])
                {
                    var company = db.CompanyModels.Find(ids);
                    if (company == null)
                        continue;
                    company.UserModels.ForEach(t => userModels.Add(t));
                }
                foreach (var ids in parsed["users"])
                {
                    if (userModels.Any(t => t.Id == ids))
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
                List<UserNameModel> us = new List<UserNameModel>();
                foreach (var user in userModels)
                {
                    var usermodel =
                        new UserNameModel
                        {
                            FIO = $"{user.Surname} {user.Name} {user.Patronymic}",
                            DocumentModels = new List<DocumentViewModel>()
                        };
                    
                    if (user.DocumentModels.Any(t=>templates.Any(r=>r.Id==t.PatternId)))
                        foreach (var document in user.DocumentModels.Where(t => templates.Any(r => r.Id == t.PatternId)))
                            usermodel.DocumentModels.Add(new DocumentViewModel(document));
                    else
                    {
                        foreach (var template in templates)
                        {
                            user.DocumentModels.Add(new DocumentModel { PatternId = template.Id, UserId = user.Id });
                            db.SaveChanges();
                            usermodel.DocumentModels.Add(new DocumentViewModel(user.DocumentModels.FirstOrDefault(t => t.PatternId == template.Id)));
                        }
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    us.Add(usermodel);
                }


                TempData["Substrate"] = substrate;
                return PartialView(us);
            }
        }
    }
}