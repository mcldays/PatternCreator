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
using WebGrease.Css.Extensions;

namespace PatternCreator.Controllers
{
    [Authorize]
    public class DiplomaController : Controller
    {
        // GET: Diploma
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Diploma()
        {
            return View();
        }
        public ActionResult GetUsers(int page = 1)
        {
            using (UserContext db = new UserContext())
            {
                ViewBag.Pages = Enumerable.Range(1, (int)Math.Ceiling((double)db.UserModels.Count() / 20));
                ViewBag.CurrentPage = page;
                IEnumerable<DiplomaUserModel> users = db.UserModels.OrderByDescending(t => t.Id).Skip(20 * (page - 1)).Take(20).ToArray().Select(t=>new DiplomaUserModel(t));
                return PartialView(users);
            }
        }
        public ActionResult FindUsers(string searchText)
        {
            using (UserContext db = new UserContext())
            {
                var terms = searchText.ToLower().Split(' ');
                IEnumerable<DiplomaUserModel> users = db.UserModels.ToArray().Where(t=>terms.All(r=>
                        string.Concat(t.Name, t.Surname, t.Patronymic).ToLower().Contains(r)))
                        .ToArray().Select(t => new DiplomaUserModel(t));

                ViewBag.Pages = new []{1};
                ViewBag.CurrentPage = 1;
                return PartialView("GetUsers",users);
            }
        }
        public ActionResult AddUser(string user)
        {
            var u = JsonConvert.DeserializeObject<DiplomaUserModel>(user);
            return PartialView(Enumerable.Repeat(u, 1).ToArray());
        }
        public ActionResult AddCompany(int id)
        {
            using (UserContext db = new UserContext())
            {
                DiplomaUserModel[] user = db.CompanyModels.Find(id)?.UserModels.ToArray()
                    .Select(t => new DiplomaUserModel(t)).ToArray();

                return PartialView("AddUser", user);

            }
        }

        public ActionResult GetMarks(string data)
        {
            dynamic da = JsonConvert.DeserializeObject(data);
            using (UserContext db = new UserContext())
            {
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                foreach (var x in da)
                {
                    UserModel user = db.UserModels.Find(int.Parse(x.userid.ToString()));
                    if ( user == null) return null;
                    SpecialtyModel spec = db.Specialties.Find(int.Parse(x.specid.ToString()));
                    if (spec == null) return null;
                    dictionary.Add(user.Id, spec.Id);
                }
                return PartialView(dictionary);
            }
        }
        [HttpPost]
        public bool RedactMarks(string data)
        {
            try
            {
                dynamic docinfo = JsonConvert.DeserializeObject(data);
                using (UserContext db = new UserContext())
                {
                    foreach (var info in docinfo)
                    {
                        Dictionary<int, string> marks = new Dictionary<int, string>();
                        foreach (var mark in info.marks)
                            marks.Add(int.Parse(mark.markid.ToString()), mark.mark.ToString());
                        DocumentModel doc = db.DocumentModels.Find(int.Parse(info.docid.ToString()));
                        if(doc==null) continue;
                        doc.HandWriteFields = JsonConvert.SerializeObject(marks);
                        db.Entry(doc).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
               
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public ActionResult GetRedactMarks(string data)
        {
            dynamic da = JsonConvert.DeserializeObject(data);
            using (UserContext db = new UserContext())
            {
                Dictionary<int, string> dictionary = new Dictionary<int, string>();
                foreach (var x in da)
                {
                    DocumentModel doc = db.DocumentModels.Find(int.Parse(x.docid.ToString()));
                    if (doc!=null)
                    {
                        dictionary.Add(doc.DocumentId, doc.HandWriteFields);
                    }
                }
                return PartialView(dictionary);
            }
        }
        public ActionResult ReadyMarks(string data)
        {
            int[] da = JsonConvert.DeserializeObject<int[]>(data);
            return PartialView(da);
        }

        [HttpPost]

        public ActionResult ReadyDocs(int[] docIds)
        {
            return PartialView(docIds);
        }
       
        [HttpPost]
        public bool DeleteDocs(int[] docIds)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    List<DocumentModel> documents = new List<DocumentModel>();
                    foreach (int docId in docIds)
                    {
                        DocumentModel doc = db.DocumentModels.Find(docId);
                        if (doc==null) continue;
                        documents.Add(doc);
                    }
                    db.DocumentModels.RemoveRange(documents);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        [HttpPost]
        public ActionResult Protocols(string protocolname, int page=1)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            using (UserContext db = new UserContext())
            {
                var models1 = db.Protocols.Where(t => t.ProtocolName.StartsWith(protocolname));
                int count = models1.Count();
                if (count < 1) { return Content("Протоколы не найдены!"); }
                ViewBag.PageCount = Enumerable.Range(1, (int)Math.Ceiling((double)count / 10));
                ViewBag.CurrentPage = page;
                ViewBag.Protocol = protocolname;
                ViewBag.DocString = null;
                var models = models1.OrderByDescending(t => t.ProtocolName).Skip(10 * (page - 1)).Take(10).ToList();
                models.ForEach(t=>dictionary.Add(t.ProtocolName, t.DocumentModels.Count(x=>x.PatternId==29)));
            }
            return PartialView("ProtocolsPartial", dictionary);
        }
        [HttpPost]
        public ActionResult GetDocsByPart(string number, int page = 1)
        {
            Dictionary<int, string[]> docs;
            using (UserContext db = new UserContext())
            {
                var pat = db.PicturesModels.Find(29);
                if (pat == null) return null;
                var documents = pat.DocumentModels.Where(t=>t.TypographicNumber.ToString().Contains(number));
                var documentModels = documents as DocumentModel[] ?? documents.ToArray();
                int count = documentModels.Count();
                if (count < 1) { return Content("Документы не найдены!" + "<div class='col-3'><button id='backProtocol' class='btn btn-primary'>Назад</button></div>"); }
                ViewBag.PageCount = Enumerable.Range(1, (int)Math.Ceiling((double)count / 20));
                ViewBag.CurrentPage = page;
                ViewBag.Protocol = null;
                ViewBag.DocString = number;
                docs = new Dictionary<int, string[]>();
                documentModels.Skip(20 * (page - 1)).Take(20).Select(t => new KeyValuePair<int, string[]>(t.DocumentId, new []{ t.TypographicNumber.ToString(),t.ProtocolName })).OrderByDescending(t => t.Key).ToList()
                    .ForEach(t => docs.Add(t.Key, t.Value));
            }
            return PartialView("GetDocs", docs);
        }

        [HttpPost]
        public ActionResult GetDocsByProtocol(string protocolname, int page = 1)
        {
            Dictionary<int,string[]> docs;
            using (UserContext db = new UserContext())
            {
                var protocol = db.Protocols.Find(protocolname);
                if (protocol == null) { return Content("Протокол не найден!" + "<div class='col-3'><button id='backProtocol' class='btn btn-primary'>Назад</button></div>"); }
                var documents = protocol.DocumentModels.Where(t=>t.PatternId==29);
                var documentModels = documents as DocumentModel[] ?? documents.ToArray();
                int count = documentModels.Count();
                if (count < 1) { return Content("Документы не найдены!"+ "<div class='col-3'><button id='backProtocol' class='btn btn-primary'>Назад</button></div>"); }
                ViewBag.PageCount = Enumerable.Range(1, (int)Math.Ceiling((double)count/ 20)); 
                ViewBag.CurrentPage = page;
                ViewBag.Protocol = protocol.ProtocolName;
                ViewBag.DocString = null;
                docs = new Dictionary<int, string[]>();
                documentModels.Skip(20 * (page - 1)).Take(20).Select(t => new KeyValuePair<int, string[]>(t.DocumentId, new[] { t.TypographicNumber.ToString(), t.ProtocolName })).OrderByDescending(t => t.Key).ToList()
                    .ForEach(t => docs.Add(t.Key, t.Value));
            }
            return PartialView("GetDocs", docs);
        }
        public ActionResult GetCompanies(int page = 1)
        {
            using (UserContext db = new UserContext())
            {
                ViewBag.Pages = Enumerable.Range(1, (int)Math.Ceiling((double)db.CompanyModels.Count() / 20));
                ViewBag.CurrentPage = page;

                IEnumerable<DiplomaCompanyModel> companies = db.CompanyModels.OrderByDescending(t => t.CompanyId).Skip(20 * (page - 1)).Take(20).ToArray().Select(t => new DiplomaCompanyModel(t));
                return PartialView(companies);
            }
        }

        
    }
}