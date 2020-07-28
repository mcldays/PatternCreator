using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
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
            return PartialView();
        }

        public ActionResult Home()
        {
            var company = SendDbUtility.GelAllCompanyList();
            return View(company);
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
                    var model = dbUse.PicturesModels.ToArray().Select(t=>new PictureModelViewModel(t)).ToArray();
                    
                    
                    ViewBag.Autotexts = dbUse.AutoTexts.ToArray();
                    return View(model);
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
                CompanyId = client.CompanyId
            };
            SendDbUtility.SendUserToDb(model);

            return RedirectToAction("Home", "Pattern");
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
                CompanyId = client.CompanyId
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


        [HttpPost]
        public ActionResult Create(HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                var Name = uploadImage.FileName.Split('.')[0];


                using (var dbUse = new UserContext())
                {
                    dbUse.PicturesModels.Add(new PictureModel
                    {
                        Image = imageData,
                        Name = Name
                    });
                    dbUse.SaveChanges();
                }

                return RedirectToAction("EditorPattern");
            }

            return View(false);
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
                    PositionModel model = picture.PositionModels.FirstOrDefault(t=>t.Id == int.Parse(block[4]));
                    if (model!=null)
                    {
                       
                             model.Id = int.Parse(block[4]);
                             model.PictureId = picId;
                             model.PosX = double.Parse(block[0].Replace('.', ','));
                             model.PosY = double.Parse(block[1].Replace('.', ','));
                             model.Width = double.Parse(block[2].Replace('.', ','));
                             model.Type = block[5];
                             model.FontSize = int.Parse(block[7]);
                             model.Height = double.Parse(block[6].Replace('.', ','));
                             model.Text = block[5].Contains("Статичный текст") ? block[8] : "";
                        
                        if (block[5] == "Статичный текст из бд")
                        {
                            dbUse.AutoTexts.AddOrUpdate(new AutoTextModel
                            {
                                Text = block[8]
                            });
                        }
                        dbUse.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        model = new PositionModel()
                        {
                            PictureId = picId,
                            PosX = double.Parse(block[0].Replace('.', ',')),
                            PosY = double.Parse(block[1].Replace('.', ',')),
                            Width = double.Parse(block[2].Replace('.', ',')),
                            Type = block[5],
                            FontSize = int.Parse(block[7]),
                            Height = double.Parse(block[6].Replace('.', ',')),
                            Text = block[5].Contains("Статичный текст") ? block[8] : ""
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
                        stamps.Height = double.Parse(stamp.Height.Replace('.', ','));
                        stamps.Width = double.Parse(stamp.Width  .Replace('.', ','));
                        stamps.PosX = double.Parse(stamp.PosX    .Replace('.', ','));
                        stamps.PosY = double.Parse(stamp.PosY.Replace('.', ','));
                        stamps.StampId = stamp.StampId;
                        dbUse.Entry(stamps).State = EntityState.Modified;
                    }
                    else
                    {
                        stamps = new StampPositions
                        {
                            PicId = picId,
                            StampId = stamp.StampId,
                            Height = double.Parse(stamp.Height.Replace('.', ',')),
                            Width = double.Parse(stamp.Width  .Replace('.', ',')),
                            PosX = double.Parse(stamp.PosX    .Replace('.', ',')),
                            PosY = double.Parse(stamp.PosY.Replace('.', ','))    
                    };
                        dbUse.StampPositions.AddOrUpdate(stamps);
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
            using (var dbUse = new UserContext())
            {
                var intID = int.Parse(pictureID);

                var model = dbUse.PicturesModels.FirstOrDefault(t => t.Id == intID);
                dbUse.PicturesModels.Remove(model);
                dbUse.PositionModels.RemoveRange(dbUse.PositionModels.Where(t => t.PictureId == intID));
                dbUse.SaveChanges();
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