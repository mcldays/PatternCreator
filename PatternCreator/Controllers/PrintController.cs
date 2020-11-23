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
    [Authorize]
    public class PrintController : Controller
    {
        
        public ActionResult GetComputedPhotos(string data, bool substrate)
        {
            var document = JsonConvert.DeserializeObject<int[]>(data);
            ViewBag.Substrate = substrate;
            using (UserContext db = new UserContext())
            {
                
                var photos = new List<DocumentPrintViewModel>();
                foreach (var modelid in document)
                {
                    DocumentModel doc;
                    doc = db.DocumentModels.Find(modelid);
                    
                    if (doc != null)
                    {
                        photos.Add(ComputePhoto.GetDocToPrint(doc));
                    }
                        
                }
                return PartialView(photos);
            }
        }
       
        [HttpPost]
        public ActionResult SaveDocuments(string data, bool substrate)
        {
            dynamic arr = JsonConvert.DeserializeObject(data);
            List<int> document = new List<int>();
            using (UserContext db = new UserContext())
            {
                foreach (var el in arr)
                {
                    DocumentModel doc = new DocumentModel
                    {
                        Date = DateTime.Parse(el.date.ToString()),
                        StartDate = DateTime.Parse(el.datestart.ToString()),
                        UserId = int.Parse(el.uid.ToString()),
                        PatternId = int.Parse(el.template.ToString()),
                        OrganizationId = int.Parse(el.organization.ToString()),
                        SpecialtyId = int.Parse(el.specialty.ToString()),
                        EducationTime = el.educationtime.ToString(),
                        ProtocolName = el.protocolname.ToString().Split('-')[0],
                        HandWriteFields = el.fields.ToString()
                    };
                    doc = db.DocumentModels.Add(doc);
                    db.SaveChanges();
                    document.Add(doc.DocumentId);
                }

            }

            string data1 = JsonConvert.SerializeObject(document);
            TempData["Substrate"] = substrate;

            return RedirectToAction("GetComputedPhotos", new { data = data1, substrate});
        }

        public JsonResult CreateProtocol(string data)
        {
            dynamic model = JsonConvert.DeserializeObject(data);
            string date = model.date.ToString();
            int days = DateTime.Parse(date).Subtract(new DateTime(2020,1,1)).Days;
            int specid = int.Parse(model.specialty.ToString());
            using (UserContext db = new UserContext())
            {
                SpecialtyModel sp = db.Specialties.Find(specid);
                string protocolname = $"{days:D3}";
                ProtocolModel pr = db.Protocols.Find(protocolname);
                if (pr != null)
                {
                    int index = 0;
                    while (pr != null)
                    {
                        index++;
                        protocolname = $"{days:D3}.{index:D2}";
                        pr = db.Protocols.Find(protocolname);
                    }
                }
                db.Protocols.Add(new ProtocolModel { ProtocolName = protocolname });
                db.SaveChanges();
                return Json(protocolname + $"-{sp.Prefix}", JsonRequestBehavior.AllowGet);
            }

            
        }
        public ActionResult GetDocuments(string data)
        {
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(data);
            var substrate = parsed["substrate"][0] == 1;
            TempData["Substrate"] = substrate;
            GetDocumentsViewModel vm = new GetDocumentsViewModel();
            vm.Companies = parsed["companies"].ToArray();
            vm.Templates = parsed["templates"].ToArray();
            vm.Users = parsed["users"].ToArray();
            return PartialView(vm);
            
        }
    }
}