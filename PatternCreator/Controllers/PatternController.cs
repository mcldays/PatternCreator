using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;

namespace PatternCreator.Controllers
{
    [Authorize]
    public class PatternController : Controller
    {
        string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "TemplateImage");
        string userphotopath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "UserPhoto");
        string stamppath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resources","Stamps");
        string respath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resourses");

        // GET: Pattern
        [AllowAnonymous]
        public ActionResult MainPage(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult MainPage(AutModelViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (SendDbUtility.verifyAutefication(model))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return string.IsNullOrEmpty(returnUrl)?RedirectToAction("Home", "Pattern") : (ActionResult)Redirect(returnUrl);
                }
                ModelState.AddModelError("","Не верное имя или пароль.");
            }
            return View(model);
        }

        public ActionResult GetTable()
        {
            CompanyViewModel[] company;
            using (UserContext db = new UserContext())
            {
                company = SendDbUtility.GelAllCompanyList();
            }
            return PartialView(company);
        }
        [HttpGet]
        public ActionResult GetUserDocs(int UserId)
        {
            using (UserContext db = new UserContext())
            {
                UserModel user = db.UserModels.Find(UserId);
                if (user==null)
                {
                    return PartialView(new UserDocs[]{});

                }
                UserDocs[] docs = user.DocumentModels.ToArray().Select(t=>new UserDocs(t)).ToArray();
                return PartialView(docs);
            }
        }

        public ActionResult EditUser(int UserId)
        {
            UserModelViewModel user;
            using (UserContext db = new UserContext())
            {
                user = new UserModelViewModel(db.UserModels.Find(UserId));
            }
            return PartialView(user);
        }
        [HttpPost]
        public ActionResult EditUser(UserModelViewModel model)
        {
            using (UserContext db = new UserContext())
            {
               var user = db.UserModels.Find(model.Id);
                user.CompanyId = model.CompanyId;
                user.Name = model.Name;
                user.Name_DativeCase = model.Name_DativeCase;
                user.Surname = model.Surname;
                user.Surname_DativeCase = model.Surname_DativeCase;
                user.Patronymic = model.Patronymic;
                user.Patronymic_DativeCase = model.Patronymic_DativeCase;
                user.CompanyId = model.CompanyId;
                user.Position = model.Position;
                user.Education = model.Education;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("GetTable");
        }
        public ActionResult Home()
        {
            var company = SendDbUtility.GelAllCompanyList();
            return View(company);
        }

        public ActionResult FindUsersByString(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Content("Пустая строка поиска");
            using (UserContext db = new UserContext())
            {
                    var terms = searchText.ToLower().Split(' ');
                    CompanyViewModel[] company = db.CompanyModels.ToArray().Select(c => new CompanyViewModel
                    {
                        CompanyName = c.CompanyName,
                        CompanyId = c.CompanyId,
                        UserViewModels = c.UserModels.ToArray().Where(t => terms.All(r =>
                                string.Concat(t.Name, t.Surname, t.Patronymic).ToLower().Contains(r)))
                            .ToArray().Select(t => new UserModelViewModel(t))
                    }).ToArray();
                    return PartialView(company);
            }
        }
        public ActionResult GetUsersByPage(int page, int id)
        {
            using (UserContext db = new UserContext())
            {
                CompanyModel company = db.CompanyModels.Find(id);
                if (company == null)
                    return Content("Компания не найдена");
                UserModelViewModel[] users = company.UserModels.Skip((page - 1)*20).Take(20).ToArray().Select(t => new UserModelViewModel(t))
                    .ToArray();
                ViewBag.Pages = Enumerable.Range(1, (int)Math.Ceiling((double)company.UserModels.Count() / 20));
                ViewBag.CurrentPage = page;
                if (users.Length < 1)
                    return Content("Компания пуста");
                return PartialView("GetUserTable", users);
            }
        }

        public ActionResult SpecialityPage()
        {
            return View();
        }
        public ActionResult SpecialityPartialPage()
        {
            using (UserContext db = new UserContext())
            {
                SpecialtyViewModel[] spvm = db.Specialties.ToList().Select(t => new SpecialtyViewModel(t)).ToArray();
                return PartialView(spvm);
            }
        }
        public int AddSpeciality(SpecialtyViewModel model)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    SpecialtyModel sp = new SpecialtyModel
                    {
                        Id = model.Id,
                        Prefix = model.Prefix,
                        SpecialityName = model.SpecialityName,
                        FieldSpecialty = model.FieldSpecialty,
                        ValidUntil = model.ValidUntil,
                        Quality = model.Quality,
                        Pictures = new List<PictureModel>(db.PicturesModels.Where(t => model.Templates.Contains(t.Id)))
                    };
                    sp = db.Specialties.Add(sp);
                    db.SaveChanges();
                    return sp.Id;
                }
            }
            catch
            {
                return 0;
            }


        }
        public bool RemoveMark(int id)
        {
            using (UserContext db = new UserContext())
            {
                MarkModel mm = db.MarkModels.Find(id);
                if (mm == null) return false;
                db.MarkModels.Remove(mm);
                db.SaveChanges();
                return true;
            }
        }
        public ActionResult NewMark()
        {
            MarkViewModel[] mv = new MarkViewModel[]
            {
                new MarkViewModel
                {
                    HoursNumber = 0,
                    Naming = "Итоговая аттестация"
                }, 
            };
            return PartialView("NewMarks",mv);
        }
        public ActionResult NewMarks(int specId)
        {
            MarkViewModel[] mv;
            using (UserContext db = new UserContext())
            {
               SpecialtyModel sp = db.Specialties.Find(specId);
               if (sp == null) return null;
               mv = sp.MarkModels.Select(t => new MarkViewModel(t)).ToArray();
               if (!mv.Any())
                   mv = new MarkViewModel[]
                   {
                       new MarkViewModel
                       {
                           HoursNumber = 0,
                           Naming = "Итоговая аттестация",
                           SpecialtyId = specId
                       },
                   };
            }
            return PartialView(mv);
        }
        public ActionResult AddMarks(MarkViewModel[] model)
        {
            if (!ModelState.IsValid)
                return null;

            try
            {
                List<int> ids = new List<int>();
                using (UserContext db = new UserContext())
                {
                    foreach (MarkViewModel markViewModel in model)
                    {
                        MarkModel sp = new MarkModel
                        {
                            Id = markViewModel.Id,
                            Naming = markViewModel.Naming,
                            HoursNumber = markViewModel.HoursNumber,
                            Number = markViewModel.Number,
                            SpecialtyId = markViewModel.SpecialtyId
                        };
                        if (sp.Id < 1)
                            sp = db.MarkModels.Add(sp);
                        else
                            db.MarkModels.AddOrUpdate(sp);
                        db.SaveChanges();
                        ids.Add(sp.Id);
                    }
                    
                    return Json(ids.ToArray(), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new int[0], JsonRequestBehavior.AllowGet);
            }


        }
        [HttpGet]
        public ActionResult UpdateSpeciality(int Id)
        {
            using (UserContext db = new UserContext())
            {
                SpecialtyModel sp = db.Specialties.Find(Id);
                SpecialtyViewModel vm = new SpecialtyViewModel(sp);
                return PartialView(vm);
            }

        }
        [HttpGet]
        public bool? RemoveSpeciality(int Id)
        {
            using (UserContext db = new UserContext())
            {
                SpecialtyModel sp = db.Specialties.Find(Id);
                if (sp == null)
                    return null;
                db.MarkModels.RemoveRange(sp.MarkModels);
                db.Specialties.Remove(sp);
                db.SaveChanges();
                return true;
            }
        }
        [HttpPost]
        public ActionResult UpdateSpeciality(SpecialtyViewModel model)
        {
            using (UserContext db = new UserContext())
            {
                SpecialtyModel sp = db.Specialties.Find(model.Id);
                sp.SpecialityName = model.SpecialityName;
                sp.Prefix = model.Prefix;
                sp.FieldSpecialty = model.FieldSpecialty;
                sp.ValidUntil = model.ValidUntil;
                sp.Quality = model.Quality;
                sp.Pictures.Clear();
                model.Templates.ToList().ForEach(tr=>sp.Pictures.Add(db.PicturesModels.Find(tr)));
                
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("SpecialityPartialPage");
            }

        }

        public ActionResult OrganizationPartialPage()
        {
            using (UserContext db = new UserContext())
            {
                OrganizationViewModel[] ovm = db.Organizations.ToList().Select(t => new OrganizationViewModel(t)).ToArray();
                return PartialView(ovm);
            }
        }
        public bool AddOrganization(OrganizationViewModel organization)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    Organization sp = new Organization
                    {
                        OrganizationId = organization.OrganizationId,
                        License = organization.License,
                        OrganizationName = organization.OrganizationName,
                        СhairmanName = organization.СhairmanName,
                        SecretaryName = organization.SecretaryName,
                        TeacherName = organization.TeacherName
                    };
                    db.Organizations.AddOrUpdate(sp);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            
                
        }
        [HttpGet]
        public ActionResult UpdateOrganization(int OrganizationId)
        {
            using (UserContext db = new UserContext())
            {
                Organization sp = db.Organizations.Find(OrganizationId);
                OrganizationViewModel vm = new OrganizationViewModel(sp);
                return PartialView(vm);
            }

        }
        [HttpGet]
        public bool? RemoveOrganization(int OrganizationId)
        {
            using (UserContext db = new UserContext())
            {
                Organization sp = db.Organizations.Find(OrganizationId);
                if (sp == null)
                    return null;
                db.Organizations.Remove(sp);
                db.SaveChanges();
                return true;
            }
        }
        [HttpPost]
        public ActionResult UpdateOrganization(OrganizationViewModel model)
        {
            using (UserContext db = new UserContext())
            {
                Organization sp = db.Organizations.Find(model.OrganizationId);
                sp.OrganizationName = model.OrganizationName;
                sp.License = model.License;
                sp.SecretaryName = model.SecretaryName;
                sp.TeacherName = model.TeacherName;
                sp.СhairmanName = model.СhairmanName;
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OrganizationPartialPage");
            }

        }
        [HttpGet]
        public JsonResult CheckSpeciality(string specialityname)
        {
            using (UserContext db = new UserContext())
            {
                SpecialtyModel sp =
                    db.Specialties.FirstOrDefault(t => t.SpecialityName.ToLower() == specialityname.ToLower());
                return Json(sp==null, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult PrintPage()
        {
            var modelsPic = SendDbUtility.GetAllPictures();
            var modelsCom = SendDbUtility.GelAllCompanyList();

            //SelectList itemsPic = new SelectList(modelsPic, "Id", "Name");
            //SelectList itemsCom = new SelectList(modelsCom, "Id", "CompanyName");
            ViewBag.Pictures = modelsPic;
            ViewBag.Companies = modelsCom;

            object[] x =
            {
                new CompanyModel(),
                new UserModel(),
                new PictureModel()
            };

            return View(x);
        }

       
        public ActionResult EditorPattern()
        {
            using (var dbUse = new UserContext())
            {
                try
                {
                    
                    
                    PictureModelViewModel[] ViewModel = dbUse.PicturesModels.ToArray().Select(t => new PictureModelViewModel(t)).ToArray();

                    ViewBag.Autotexts = dbUse.AutoTexts.ToArray();
                    return View(ViewModel);
                }
                catch (Exception e)
                {
                    return View();
                }
            }
        }



        public ActionResult CompanyToDb(CreateCompanyModel newCompany)
        {
            SendDbUtility.sendCompanyModel(newCompany);


            return RedirectToAction("Home", "Pattern");
        }


        public ActionResult UserToDb(UserModelViewModel client)
        {
            UserModel model = new UserModel
            {
                Name = client.Name,
                Name_DativeCase = client.Name_DativeCase,
                Surname = client.Surname,
                Surname_DativeCase = client.Surname_DativeCase,
                Patronymic = client.Patronymic,
                Patronymic_DativeCase = client.Patronymic_DativeCase,
                CompanyId = client.CompanyId,
                Position = client.Position,
                Education = client.Education
            };
            SendDbUtility.SendUserToDb(model);

            return RedirectToAction("Home", "Pattern");
        }
        [AllowAnonymous]
        public int UserToDbApi(string user)
        {
            UserModel user1 = JsonConvert.DeserializeObject<UserModel>(user);
            try
            {
                using (UserContext db = new UserContext())
                {
                    user1 = db.UserModels.Add(user1);
                    db.SaveChanges();
                    return user1.Id;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            
        }

        public bool RemoveCompany(int id)
        {
            return SendDbUtility.DeleteCompany(id); 
        }


        public bool RemoveUser(int id)
        {
            return SendDbUtility.DeleteUserById(id);
        }

        public bool SaveUser(UserModelViewModel client)
        {
            UserModel model = new UserModel
            {
                Id = client.Id,
                Name = client.Name,
                Name_DativeCase = client.Name_DativeCase,
                Surname = client.Surname,
                Surname_DativeCase = client.Surname_DativeCase,
                Patronymic = client.Patronymic,
                Patronymic_DativeCase = client.Patronymic_DativeCase,
                CompanyId = client.CompanyId,
                Position = client.Position,
                Education = client.Education
            };
            return SendDbUtility.UpdateUser(model);
        }


        public ActionResult SaveCompany(CompanyModel model)
        {
            SendDbUtility.SaveCompany(model);


            return RedirectToAction("Home", "Pattern");
        }

        public ActionResult FindUsersParital(string name)
        {
            using (var dbUse = new UserContext())
            {
                var data = dbUse.UserModels.Where(t => t.Name.Contains(name)).ToList();
                return PartialView(data);
            }
        }


        public bool EditUserParital(int userId, string Name, string Surname, string Patronymic, int CompanyId)
        {
            var model = new UserModel
            {
                Id = userId,
                CompanyId = CompanyId,
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic
            };

            using (var dbUse = new UserContext())
            {
                dbUse.UserModels.AddOrUpdate(model);
                dbUse.SaveChanges();
            }


            return true;
        }
        

        public ActionResult GetStamps()
        {
            using (var dbUse = new UserContext())
            {
               StampModel[] stamps = dbUse.StampModels.ToArray();
               return PartialView(stamps);
            }
            
        }
        [AllowAnonymous]
        public FileResult GetPictureTemplate(int id)
        {
            using (var dbUse = new UserContext())
            {
                var image = dbUse.PicturesModels.Find(id);
                if (image == null)
                    return null;
                if (string.IsNullOrEmpty(image.PathToImg))
                    return null;

                return File(System.IO.File.ReadAllBytes(Path.Combine(path, image.PathToImg)), "image/jpeg");
            }

        }
        [AllowAnonymous]
        public FileResult GetStaticImage(int id)
        {
            using (var dbUse = new UserContext())
            {
                var image = dbUse.StaticImageModel.Find(id);
                if (image == null)
                    return null;
                if (string.IsNullOrEmpty(image.PathToImg))
                    return null;

                return File(System.IO.File.ReadAllBytes(Path.Combine(path, image.PathToImg)), "image/jpeg");
            }

        }
        [AllowAnonymous]
        public FileResult GetEagle()
        {
            return File(System.IO.File.ReadAllBytes(Path.Combine(respath,"image1.png" )), "image/jpeg");
        }

        public ActionResult DeleteStamp(int id)
        {
            using (var dbUse = new UserContext())
            {
                StampModel st = dbUse.StampModels.Find(id);
                if (st == null)
                    return RedirectToAction("EditorPattern");
                dbUse.StampModels.Remove(st);
                dbUse.SaveChanges();
                return RedirectToAction("GetStamps");
            }

        }
        [HttpPost]
        public bool UpdateStampName(StampModel stamp)
        {
            using (var dbUse = new UserContext())
            {
                StampModel st = dbUse.StampModels.Find(stamp.Id);
                if (st == null)
                    return false;
                st.Name = stamp.Name;
                dbUse.Entry(st).State = EntityState.Modified;
                dbUse.SaveChanges();
            }
            return true;
        }
        [HttpPost]
        public ActionResult AddStamp(HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                string Name = Path.GetFileNameWithoutExtension(uploadImage.FileName);
                using (var dbUse = new UserContext())
                {
                    dbUse.StampModels.Add(new StampModel
                    {
                        Image = imageData,
                        Name = Name
                    });
                    dbUse.SaveChanges();
                }

                return RedirectToAction("GetStamps");
            }

            return RedirectToAction("EditorPattern");
        }

        public bool AddPhoto(HttpPostedFileBase uploadImage, int id)
        {
            if (ModelState.IsValid && uploadImage != null)
            {

                using (Image image = Image.FromStream(uploadImage.InputStream))
                {
                    var Name = id.ToString() + Path.GetExtension(uploadImage.FileName);
                    Directory.CreateDirectory(userphotopath);
                    Directory.GetFiles(userphotopath).Where(t => t.StartsWith(Path.Combine(userphotopath, id.ToString()))).ToList().ForEach(t => System.IO.File.Delete(t));
                    image.Save(Path.Combine(userphotopath, Name));
                }
                return true;
            }

            return false;
        }
        public string AddOrganizationStamp(HttpPostedFileBase uploadImage, int OrganizationId, StampType type=StampType.Stamp)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                string Name = string.Empty;

                using (UserContext db = new UserContext())
                {
                    Organization org = db.Organizations.Find(OrganizationId);
                    if (org == null)
                        return string.Empty;
                    Directory.CreateDirectory(stamppath);
                    switch (type)
                    {
                        case StampType.Stamp:
                            if (!string.IsNullOrEmpty(org.Stamp) && System.IO.File.Exists(Path.Combine(stamppath, org.Stamp)))
                                System.IO.File.Delete(Path.Combine(stamppath, org.Stamp));
                            Name = Path.GetRandomFileName() + ".png";
                            using (Image image = Image.FromStream(uploadImage.InputStream))
                                image.Save(Path.Combine(stamppath, Name));
                            org.Stamp = Name;
                            break;
                        case StampType.СhairmanSignature:
                            if (!string.IsNullOrEmpty(org.СhairmanSignature) && System.IO.File.Exists(Path.Combine(stamppath, org.СhairmanSignature)))
                                System.IO.File.Delete(Path.Combine(stamppath, org.СhairmanSignature));
                            Name = Path.GetRandomFileName() + ".png";
                            using (Image image = Image.FromStream(uploadImage.InputStream))
                                image.Save(Path.Combine(stamppath, Name));
                            org.СhairmanSignature = Name;
                            break;
                        case StampType.TeacherSignature:
                            if (!string.IsNullOrEmpty(org.TeacherSignature) && System.IO.File.Exists(Path.Combine(stamppath, org.TeacherSignature)))
                                System.IO.File.Delete(Path.Combine(stamppath, org.TeacherSignature));
                            Name = Path.GetRandomFileName() + ".png";
                            using (Image image = Image.FromStream(uploadImage.InputStream))
                                image.Save(Path.Combine(stamppath, Name));
                            org.TeacherSignature = Name;
                            break;
                        case StampType.SecretarySignature:
                            if (!string.IsNullOrEmpty(org.SecretarySignature) && System.IO.File.Exists(Path.Combine(stamppath, org.SecretarySignature)))
                                System.IO.File.Delete(Path.Combine(stamppath, org.SecretarySignature));
                            Name = Path.GetRandomFileName() + ".png";
                            using (Image image = Image.FromStream(uploadImage.InputStream))
                                image.Save(Path.Combine(stamppath, Name));
                            org.SecretarySignature = Name;
                            break;
                    }
                    db.SaveChanges();
                }
                return Name;
            }

            return string.Empty;
        }

        public enum StampType
        {
            Stamp,
            СhairmanSignature,
            TeacherSignature,
            SecretarySignature
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                Image image = Image.FromStream(uploadImage.InputStream);

                var Name = Path.GetRandomFileName() + Path.GetExtension(uploadImage.FileName);
                image.Save(Path.Combine(path, Name));

                using (var dbUse = new UserContext())
                {
                    dbUse.PicturesModels.Add(new PictureModel
                    {
                        PathToImg = Name,
                        Name = Path.GetFileNameWithoutExtension(uploadImage.FileName),
                        NaturalHeight = 297,
                        NaturalWidth = 210
                    });
                    dbUse.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError("","Возникла ошибка при загрузке шаблона. Попробуйте ещё раз.");
            }
            return RedirectToAction("EditorPattern");
        }
        
        public FileResult GetUserPhoto(string id)
        {
            if(string.IsNullOrEmpty(id))
                return File(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resourses", "couldnotfound.png"), "image/jpeg");
            try
            {
                string p = Path.Combine(userphotopath, id.ToString());
                var f = Directory.GetFiles(userphotopath).FirstOrDefault(t => t.StartsWith(p));
                if (f == null)
                    return File(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resourses", "couldnotfound.png"), "image/jpeg");
                return File(System.IO.File.ReadAllBytes(f), "image/jpeg");
            }
            catch (Exception e)
            {
                return null;
            }
           

        }
        public FileResult GetOrgStamp(string f)
        {
            if (string.IsNullOrEmpty(f))
                return File(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resourses", "couldnotfound.png"), "image/jpeg");
            try
            {
                string p = Path.Combine(stamppath, f);
                if (!System.IO.File.Exists(p))
                    return File(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Resourses", "couldnotfound.png"), "image/jpeg");
                return File(System.IO.File.ReadAllBytes(p), "image/jpeg");
            }
            catch (Exception e)
            {
                return null;
            }


        }
        [HttpPost]
        public bool SetBlocks(string data)
        {            
            
            SetBlockModel b = JsonConvert.DeserializeObject<SetBlockModel>(data);
            var picId = Int32.Parse(b.picId);
            List<List<string>> a;

            a = b.bounds;
            
                using (var dbUse = new UserContext())
                {
                    var pic = dbUse.PicturesModels.Find(picId);
                    pic.Name = b.Name;
                    pic.NaturalHeight = int.Parse(b.NaturalHeight);
                    pic.NaturalWidth = int.Parse(b.NaturalWidth);
                    if (a.Count == 0)
                        dbUse.PositionModels.RemoveRange(pic.PositionModels);
                    if (!b.stamps.Any())
                        dbUse.StampPositions.RemoveRange(pic.StampPositions);
                    dbUse.Entry(pic).State = EntityState.Modified;
                    dbUse.SaveChanges();
                }
                if(a.Count == 0&& !b.stamps.Any())
                    return false;

            using (var dbUse = new UserContext())
            {
                PictureModel picture = dbUse.PicturesModels.Find(picId);
                
                foreach (var block in a)
                {
                    PositionModel model = picture?.PositionModels.FirstOrDefault(t=>t.Id == int.Parse(block[4]));
                    if (model!=null)
                    {
                       
                             model.Id = int.Parse(block[4]);
                             model.PictureId = picId;
                             model.PosX = float.Parse(block[0].Replace('.', ','));
                             model.PosY = float.Parse(block[1].Replace('.', ','));
                             model.Width = float.Parse(block[2].Replace('.', ','));
                             model.Type = block[5];
                             model.FontSize = int.Parse(block[7]);
                             model.Height = float.Parse(block[6].Replace('.', ','));
                             model.Text = block[5].Contains("Статичный текст") ? block[8].Replace("\n", "*newline*") : "";
                             model.FontWeight = block[9];
                             model.Alignment = block[10];


                        if (block[5] == "Статичный текст из бд")
                        {
                            dbUse.AutoTexts.AddOrUpdate(new AutoTextModel
                            {
                                Text = block[8].Replace("\n","*newline*")
                            });
                        }
                        dbUse.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        model = new PositionModel()
                        {
                            PictureId = picId,
                            PosX = float.Parse(block[0].Replace('.', ',')),
                            PosY = float.Parse(block[1].Replace('.', ',')),
                            Width = float.Parse(block[2].Replace('.', ',')),
                            Type = block[5],
                            FontSize = int.Parse(block[7]),
                            Height = float.Parse(block[6].Replace('.', ',')),
                            Text = block[5].Contains("Статичный текст") ? block[8] : "",
                            FontWeight = block[9],
                            Alignment = block[10]
                    };
                        if (block[5] == "Статичный текст из бд")
                        {
                            dbUse.AutoTexts.AddOrUpdate(new AutoTextModel
                            {
                                Text = block[8]
                            });
                        }
                        dbUse.PositionModels.AddOrUpdate(model);
                    }
                   
                    
                }
                foreach (var stamp in b.stamps)
                {
                    StampPositions stamps = picture.StampPositions.FirstOrDefault(t => t.StampPositionId == stamp.StampPositionId);
                    if (stamps!=null)
                    {
                        stamps.Height = float.Parse(stamp.Height.Replace('.', ','));
                        stamps.Width = float.Parse(stamp.Width  .Replace('.', ','));
                        stamps.PosX = float.Parse(stamp.PosX    .Replace('.', ','));
                        stamps.PosY = float.Parse(stamp.PosY.Replace('.', ','));
                        stamps.Type = stamp.Type;
                        dbUse.Entry(stamps).State = EntityState.Modified;
                    }
                    else
                    {
                        stamps = new StampPositions
                        {
                            PicId = picId,
                            Type = stamp.Type,
                            Height = float.Parse(stamp.Height.Replace('.', ',')),
                            Width = float.Parse(stamp.Width  .Replace('.', ',')),
                            PosX = float.Parse(stamp.PosX    .Replace('.', ',')),
                            PosY = float.Parse(stamp.PosY.Replace('.', ','))    
                    };
                        dbUse.StampPositions.AddOrUpdate(stamps);
                    }
                }
                foreach (var photo in b.photos)
                {
                    PhotoModel photos = picture.PhotoModels.FirstOrDefault(t => t.PhotoModelId == photo.PhotoModelId);
                    if (photos != null)
                    {
                        photos.Height = float.Parse(photo.Height.Replace('.', ','));
                        photos.Width = float.Parse(photo.Width.Replace('.', ','));
                        photos.PosX = float.Parse(photo.PosX.Replace('.', ','));
                        photos.PosY = float.Parse(photo.PosY.Replace('.', ','));
                        photos.PhotoModelId = photo.PhotoModelId;
                        dbUse.Entry(photos).State = EntityState.Modified;
                    }
                    else
                    {
                        photos = new PhotoModel
                        {
                            PicId = picId,
                            PhotoModelId = photo.PhotoModelId,
                            Height = float.Parse(photo.Height.Replace('.', ',')),
                            Width = float.Parse(photo.Width.Replace('.', ',')),
                            PosX = float.Parse(photo.PosX.Replace('.', ',')),
                            PosY = float.Parse(photo.PosY.Replace('.', ','))
                        };
                        dbUse.PhotoModel.AddOrUpdate(photos);
                    }
                }
                foreach (var photo in b.images)
                {
                    StaticImageModel photos = picture.StaticImageModels.FirstOrDefault(t => t.StaticImageModelId == photo.StaticImageModelId);
                    if (photos != null)
                    {
                        photos.Height = float.Parse(photo.Height.Replace('.', ','));
                        photos.Width = float.Parse(photo.Width.Replace('.', ','));
                        photos.PosX = float.Parse(photo.PosX.Replace('.', ','));
                        photos.PosY = float.Parse(photo.PosY.Replace('.', ','));
                        photos.StaticImageModelId = photo.StaticImageModelId;
                        dbUse.Entry(photos).State = EntityState.Modified;
                    }
                    else
                    {
                        photos = new StaticImageModel
                        {
                            PicId = picId,
                            StaticImageModelId = photo.StaticImageModelId,
                            Height = float.Parse(photo.Height.Replace('.', ',')),
                            Width = float.Parse(photo.Width.Replace('.', ',')),
                            PosX = float.Parse(photo.PosX.Replace('.', ',')),
                            PosY = float.Parse(photo.PosY.Replace('.', ','))
                        };
                        dbUse.StaticImageModel.AddOrUpdate(photos);
                    }
                }
                if (a.Count != 0)
                    dbUse.PositionModels.RemoveRange(picture.PositionModels.Where(t => dbUse.Entry(t).State == EntityState.Unchanged));
                if (b.stamps.Any())
                    dbUse.StampPositions.RemoveRange(picture.StampPositions.Where(t => dbUse.Entry(t).State == EntityState.Unchanged));
                dbUse.SaveChanges();
            }

            SendDbUtility.UpdateImage(int.Parse(b.Id), b.Name);
            return true;
        }


        public bool DeletePicture(string pictureID)
        {
            try
            {
                using (var dbUse = new UserContext())
                {
                    var intID = int.Parse(pictureID);

                    var model = dbUse.PicturesModels.Find(intID);
                    if (model==null)
                    {
                        return true;
                    }
                    string fullpath = Path.Combine(path, model.PathToImg);
                    if (System.IO.File.Exists(fullpath))
                        System.IO.File.Delete(fullpath);
                    
                    dbUse.PicturesModels.Remove(model);
                    dbUse.PositionModels.RemoveRange(dbUse.PositionModels.Where(t => t.PictureId == intID));
                    dbUse.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return true;
            }
            
        }


        public ActionResult UpdatePicture(string Name, int Id)
        {
            SendDbUtility.UpdateImage(Id, Name);

            return RedirectToAction("EditorPattern", "Pattern");
        }


        public ActionResult PrintParital()
        {
            using (var db = new UserContext())
            {
                var data = db.CompanyModels.ToArray().Select(t => new CompanyViewModel
                {
                    CompanyName = t.CompanyName,
                    CompanyId = t.CompanyId,
                    UserViewModels = t.UserModels.Select(u => new UserModelViewModel(u))
                }).ToList();
                return PartialView(data);
            }
        }
    }
}