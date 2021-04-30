using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        [AllowAnonymous]
        [HttpPost]
        public string SaveDocApi(string data)
        {
            return GetSavedDocsJson(data);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDocumentApi(int docId)
        {
            using (UserContext db = new UserContext())
            {
                DocumentModel doc;
                doc = db.DocumentModels.Find(docId);
                if (doc == null)
                    return HttpNotFound();
                return View(ComputePhoto.GetDocToPrint(doc));
            }
        }


        private string GetSavedDocsJson(string data)
        {
            dynamic arr = JsonConvert.DeserializeObject(data);
            List<int> document = new List<int>();
            List<string> protocolname = new List<string>();
            List<DocumentModel> docs = new List<DocumentModel>();
            using (UserContext db = new UserContext())
            {
                foreach (var el in arr)
                {
                    DocumentModel doc;

                    if (el.protocolname.ToString().Contains("000.0"))
                    {
                        doc = new DocumentModel
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
                        PictureModel pic = db.PicturesModels.Find(doc.PatternId);
                        doc.BlankNumber = pic.BlankNumber;
                        pic.BlankNumber++;
                        db.Entry(pic).State = EntityState.Modified;
                        db.SaveChanges();
                        document.Add(doc.DocumentId);
                        return JsonConvert.SerializeObject(document);
                    }
                    ProtocolModel pr = db.Protocols.Find(el.protocolname.ToString().Split('-')[0]);
                    if (pr == null)
                        pr = db.Protocols.Add(new ProtocolModel { ProtocolName = el.protocolname.ToString().Split('-')[0] });
                    
                    if (pr != null && pr.DocumentModels.Any())
                    {
                        if (!protocolname.Contains(el.protocolname.ToString().Split('-')[0]))
                            protocolname.Add(el.protocolname.ToString().Split('-')[0]);

                        doc = pr.DocumentModels.FirstOrDefault(t => t.PatternId == int.Parse(el.template.ToString()) &&
                                                                    t.UserId == int.Parse(el.uid.ToString()));
                        if (doc != null)
                        {
                            doc.Date = DateTime.Parse(el.date.ToString());
                            doc.StartDate = DateTime.Parse(el.datestart.ToString());
                            doc.UserId = int.Parse(el.uid.ToString());
                            doc.PatternId = int.Parse(el.template.ToString());
                            doc.OrganizationId = int.Parse(el.organization.ToString());
                            doc.SpecialtyId = int.Parse(el.specialty.ToString());
                            doc.EducationTime = el.educationtime.ToString();
                            doc.ProtocolName = el.protocolname.ToString().Split('-')[0];
                            doc.HandWriteFields = el.fields.ToString();
                            db.Entry(doc).State = EntityState.Modified;
                        }
                        else
                        {
                            doc = new DocumentModel
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
                            PictureModel pic = db.PicturesModels.Find(doc.PatternId);
                            doc.BlankNumber = pic.BlankNumber;
                            pic.BlankNumber++;
                            db.Entry(pic).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        doc = new DocumentModel
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
                        PictureModel pic = db.PicturesModels.Find(doc.PatternId);
                        doc.BlankNumber = pic.BlankNumber;
                        pic.BlankNumber++;
                        db.Entry(pic).State = EntityState.Modified;
                    }
                    docs.Add(doc);
                }
                foreach (string n in protocolname)
                {
                    ProtocolModel protocol = db.Protocols.Find(n);
                    if (protocol != null && db.Entry(protocol).State != EntityState.Added)
                        foreach (DocumentModel model in protocol.DocumentModels.ToArray())
                            if (db.Entry(model).State == EntityState.Unchanged)
                                db.DocumentModels.Remove(model);
                }
                db.SaveChanges();
                foreach (DocumentModel doc in docs)
                    document.Add(doc.DocumentId);
            }
            return JsonConvert.SerializeObject(document);
        }
       
        [HttpPost]
        public ActionResult SaveDocuments(string data, bool substrate)
        {
            string data1 = GetSavedDocsJson(data);
            TempData["Substrate"] = substrate;
            return RedirectToAction("GetComputedPhotos", new { data = data1, substrate});
        }
        public ActionResult DiplomaPartial(string data, bool isdiploma, bool isapplication, bool ismarks, bool substrate)
        {
            ViewBag.IsDiploma = isdiploma;
            ViewBag.IsApplication = isapplication;
            ViewBag.IsMark = ismarks;
            ViewBag.Substrate = substrate;
            Dictionary<int, int> ar = SaveDiploma(data);
            return PartialView("DiplomaPartial", ar);
        }
        public ActionResult ReadyDiploma(string data, bool isdiploma, bool isapplication, bool ismarks, bool substrate)
        {
            ViewBag.IsDiploma = isdiploma;
            ViewBag.IsApplication = isapplication;
            ViewBag.IsMark = ismarks;
            ViewBag.Substrate = substrate;
            Dictionary<int, int> ar = new Dictionary<int, int>();

            using (UserContext db = new UserContext())
            {
                int[] ids = JsonConvert.DeserializeObject<int[]>(data);
                int i = 0;
                foreach (var id in ids)
                {
                    DocumentModel doc = db.DocumentModels.Find(id);
                    if (doc == null) continue;
                    ar.Add(i++, doc.DocumentId);
                }
            }
            return PartialView("DiplomaPartial", ar);
        }
        private Dictionary<int, int> SaveDiploma(string data)
        {
            dynamic arr = JsonConvert.DeserializeObject(data);
            List<DocumentModel> documents = new List<DocumentModel>();
            Dictionary<int, int> rd = new Dictionary<int, int>();
            using (UserContext db = new UserContext())
            {
                foreach (dynamic obj in arr)
                {
                    int docid = int.Parse(obj.docid.ToString());
                    Dictionary<int, string> marks = new Dictionary<int, string>();
                    foreach (var mark in obj.marks)
                        marks.Add(int.Parse(mark.markid.ToString()), mark.mark.ToString());
                    if (!marks.Any())
                    {
                        DocumentModel d = db.DocumentModels.Find(docid);
                        if (d!=null)
                        {
                            marks = JsonConvert.DeserializeObject<Dictionary<int, string>>(d.HandWriteFields);
                        }
                    }
                    DocumentModel doc = new DocumentModel
                    {
                        DocumentId = docid,
                        Date = DateTime.Parse(obj.date.ToString()),
                        StartDate = DateTime.Parse(obj.datestart.ToString()),
                        UserId = int.Parse(obj.uid.ToString()),
                        PatternId = 29,
                        CertificationWork = obj.certificationwork.ToString(),
                        OrganizationId = int.Parse(obj.organization.ToString()),
                        SpecialtyId = int.Parse(obj.specialty.ToString()),
                        ProtocolName = obj.protocolname.ToString().Split('-')[0],
                        HandWriteFields = JsonConvert.SerializeObject(marks),
                        TypographicNumber = long.Parse(obj.typographicnumber.ToString().Length<12?"100000000000":obj.typographicnumber.ToString())
                    };
                    PictureModel pic = db.PicturesModels.Find(doc.PatternId);
                    doc.BlankNumber = pic.BlankNumber;
                    if (doc.DocumentId==0)
                    {
                        pic.BlankNumber++;
                        db.Entry(pic).State = EntityState.Modified;
                    }
                    db.DocumentModels.AddOrUpdate(doc);
                    
                    documents.Add(doc);
                }
                db.SaveChanges();
                documents.ToList().ForEach(t=>rd.Add(t.UserId, t.DocumentId));
                return rd;
            }
            
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