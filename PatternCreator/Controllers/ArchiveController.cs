using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;

namespace PatternCreator.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index()
        {
            return View();
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
        [HttpPost]
        public ActionResult Docs(int docId)
        {
            DocumentModel[] model;
            using (UserContext db = new UserContext())
            {
                DocumentModel doc = db.DocumentModels.Find(docId);
                if (doc==null)
                    model = db.DocumentModels.Where(t=>t.DocumentId.ToString().StartsWith(docId.ToString())).ToArray();
                else
                    model = new[] {doc};
            }
            return PartialView("DocsPartial", model);
        }
        [HttpGet]
        public ActionResult GetDocsByProtocol(string protocolname)
        {
            int[] docs;
            using (UserContext db = new UserContext())
            {
               docs = db.Protocols.Find(protocolname).DocumentModels.ToArray().Select(t=>t.DocumentId).ToArray();
            }

            return PartialView("GetDocs",docs);
        }
        [HttpGet]
        public ActionResult GetDocs(int docId)
        {
            int[] docs;
            using (UserContext db = new UserContext())
            {
               DocumentModel doc = db.DocumentModels.Find(docId);
                if (doc == null)
                    docs = null;
                else
                    docs = new[] {docId};
            }
            return PartialView(docs);
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