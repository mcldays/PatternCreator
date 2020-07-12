using System;
using System.Collections.Generic;
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
                    var model = dbUse.PicturesModels.ToList();
                    var positions = dbUse.PositionModels.ToList();
                    var list = new object[]
                    {
                        model,
                        positions
                    };
                    return View(list);
                }
                catch (Exception e)
                {
                    return View(new object[] {new List<PictureModel>(), new List<PositionModel>()});
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
            var b = JsonConvert.DeserializeObject<SetBlockModel>(data);
            var picId = Int32.Parse(b.picId);
            List<List<string>> a;

            a = b.bounds;
            if (a.Count == 0)
            {
                using (var dbUse = new UserContext())
                {
                    dbUse.PositionModels.RemoveRange(dbUse.PositionModels.Where(t => t.PictureId == picId));
                    dbUse.SaveChanges();
                }

                return false;
            }


            using (var dbUse = new UserContext())
            {
                dbUse.PositionModels.RemoveRange(dbUse.PositionModels.Where(t=>t.PictureId == picId));
                dbUse.SaveChanges();
                foreach (var block in a)
                {
                    PositionModel model;
                    if (int.TryParse(block[4], out int id))
                    {
                        model = new PositionModel()
                        {
                            Id = id,
                            PictureId = picId,
                            PosX = double.Parse(block[0].Replace('.', ',')),
                            PosY = double.Parse(block[1].Replace('.', ',')),
                            Width = double.Parse(block[2].Replace('.', ',')),
                            Type = block[5],
                            FontSize = int.Parse(block[7]),
                            Height = double.Parse(block[6].Replace('.', ',')),
                            Text = block[5] == "Статичный текст" ? block[8] : ""
                        };
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
                            Text = block[5] == "Статичный текст" ? block[8] : ""
                        };
                    }

                    dbUse.PositionModels.AddOrUpdate(model);
                }
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


        public ActionResult PrintParital(string name)
        {
            using (var dbUse = new UserContext())
            {
                var data = dbUse.UserModels.Where(t =>
                    t.Name.Contains(name) || t.Surname.Contains(name) || t.Patronymic.Contains(name)).ToList();
                return PartialView(data);
            }
        }
    }
}