using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;

namespace PatternCreator.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index()
        {
            return RedirectToAction("Protocols");
        }
        [HttpGet]
        public ActionResult Protocols()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Protocols(string protocolname)
        {
            ProtocolModel[] model;
            using (UserContext db = new UserContext())
            {
                model = db.Protocols.Where(t => t.ProtocolName.StartsWith(protocolname)).ToArray();
            }
            return PartialView("ProtocolsPartial", model);
        }
        [HttpGet]
        public JsonResult Docs(string data)
        {
            int[] model;
            using (UserContext db = new UserContext())
            {
                if (!data.IsInt())
                {
                    return null;
                }

                int d = Convert.ToInt32(data);
                
                    model = db.DocumentModels.Where(t => t.DocumentId.ToString().StartsWith(data.ToString()) && t.PatternId != 29).Take(10).Select(t => t.DocumentId).ToArray();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDocsByProtocol(string protocolname)
        {
            int[] docs;
            using (UserContext db = new UserContext())
            {
               docs = db.Protocols.Find(protocolname).DocumentModels.Where(t=>t.PatternId!=29).ToArray().Select(t=>t.DocumentId).ToArray();
            }

            return PartialView("GetDocs",docs);
        }
        [HttpGet]
        public bool DeleteProtocol(string protocolname)
        {
            int[] docs;
            using (UserContext db = new UserContext())
            {
                ProtocolModel d = db.Protocols.Find(protocolname);
                if (d!=null)
                {
                    db.DocumentModels.RemoveRange(d.DocumentModels);
                    db.Protocols.Remove(d);
                    db.SaveChanges();
                }
                return true;
            }

        }
        [HttpPost]
        public ActionResult GetDocs(int[] DocIds)
        {
            //int[] docs;
            return PartialView(DocIds);
            //using (UserContext db = new UserContext())
            //{
            //   DocumentModel doc = db.DocumentModels.Find(docId);
            //    if (doc == null)
            //        docs = null;
            //    else
            //        docs = new[] {docId};
            //}
            //return PartialView(docs);
        }
        [HttpGet]
        public bool RemoveDocs(string DocIds)
        {
            int[] docsId = JsonConvert.DeserializeObject<int[]>(DocIds);
            using (UserContext db = new UserContext())
            {                
                    var docs = db.DocumentModels.Where(t=> docsId.Contains(t.DocumentId));
                    db.DocumentModels.RemoveRange(docs.ToArray());
                    db.SaveChanges();
            }
            //return PartialView(docs);
            return true;
        }
        [HttpPost]
        public ActionResult UpdateDoc(string data, bool substrate)
        {
            dynamic arr = JsonConvert.DeserializeObject(data);
            List<int> document = new List<int>();
            using (UserContext db = new UserContext())
            {
                foreach (var el in arr)
                {
                    DocumentModel doc = db.DocumentModels.Find(int.Parse(el.docId.ToString()));
                    doc.Date = DateTime.Parse(el.date.ToString());
                    doc.SpecialtyId = Convert.ToInt32(el.specialty);
                    doc.StartDate = DateTime.Parse(el.datestart.ToString());
                    doc.EducationTime = el.educationtime.ToString();
                    doc.ProtocolName = el.protocolname.ToString().Split('-')[0];
                    doc.HandWriteFields = el.fields.ToString();
                    db.Entry(doc).State = EntityState.Modified;
                    db.SaveChanges();
                    document.Add(doc.DocumentId);

                }

            }
            string data1 = JsonConvert.SerializeObject(document);
            return RedirectToAction("GetComputedPhotos","Print", new { data = data1, substrate });
        }
    }
}