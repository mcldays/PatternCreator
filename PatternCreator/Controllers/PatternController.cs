using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PatternCreator.Models;
using PatternCreator.Utilities;

namespace PatternCreator.Controllers
{
    public class PatternController : Controller
    {
        // GET: Pattern
        public ActionResult MainPage(AutModel model)
        {
            if (Utilities.SendDbUtility.verifyAutefication(model) == true)
            {
                return RedirectToAction("Home", "Pattern");
            }


        return View();
        }

        public ActionResult Home()
        {
            List<string> Groups = new List<string>();
            var models = Utilities.SendDbUtility.GelAllCompanyList();
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var company in models)
            {
              

                items.Add(new SelectListItem { Text = company.CompanyName, Value = company.Id.ToString() });

            }



            ViewBag.CompanyName = items;
           


            object[] x = new object[]
            {
                new CompanyModel(), 
                new UserModel(), 
                
            };

            return View(x);
        }

        public ActionResult PrintPage()
        {
            return View();
        }

        public ActionResult EditorPattern()
        {

            using (UserContext dbUse = new UserContext())
            {
                var model = dbUse.PicturesModels.ToList<PictureModel>();
                var positions = dbUse.PositionModels.ToList();
                var list = new object[]
                {
                    model,
                    positions
                };



                return View(list);
            }

        }


        public ActionResult CompanyToDb(string CompanyName)
        {
            SendDbUtility.sendCompanyModel(CompanyName);


            return RedirectToAction("Home",  "Pattern");
        }


        public ActionResult UserToDb(UserModel model, string CompanyName)
        {
            model.CompanyId = int.Parse(CompanyName);
            Utilities.SendDbUtility.SendUserToDb(model);

            return RedirectToAction("Home", "Pattern");
        }

        public bool RemoveCompany(int id)
        {
            SendDbUtility.DeleteAllUsersInCompany(id);
            SendDbUtility.DeleteCompany(id);
            
            return true;
        }


        public bool RemoveUser(int id)
        {
            SendDbUtility.DeleteUserById(id);
            return true;
        }

        public ActionResult SaveUser(UserModel model)
        {
            Utilities.SendDbUtility.UpdateUser(model);


            return RedirectToAction("Home", "Pattern");
        }



        public ActionResult SaveCompany(CompanyModel model)
        {

            Utilities.SendDbUtility.SaveCompany(model);



            return RedirectToAction("Home", "Pattern");
        }

        public ActionResult FindUsersParital(string name)
        {
            using (UserContext dbUse= new UserContext())
            {
               var data= dbUse.UserModels.Where(t => t.Name == name).ToList<UserModel>();
               return PartialView(data);
            }


        }


        public bool EditUserParital(int userId, string Name, string Surname, string Patronymic, int CompanyId)
        {
            UserModel model = new UserModel()
            {
                Id = userId,
                CompanyId = CompanyId,
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic
            };

            using (UserContext dbUse = new UserContext()) 
            {
                dbUse.UserModels.AddOrUpdate(model);
                dbUse.SaveChanges();
            }



            return true;
        }



        [HttpPost]
        public ActionResult Create(PictureModel pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
              
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
             
                pic.Image = imageData;

                var Name = uploadImage.FileName.Split('.')[0];


                using (UserContext dbUse = new UserContext())
                {
                    dbUse.PicturesModels.Add(new PictureModel()
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
            var a = JsonConvert.DeserializeObject<List<List<string>>>(data);
            using (var dbUse = new UserContext())
            {
                foreach (var block in a)
                {
                    try
                    {
                        var id = int.Parse(block[4]);
                        dbUse.PositionModels.AddOrUpdate(new PositionModel()
                        {
                            Id = id,
                            PictureId = int.Parse(block[3]),
                            PosX = double.Parse(block[0]),
                            PosY = double.Parse(block[1]),
                            Width = double.Parse(block[2]),
                            Type = block[5]
                        });
                    }
                    catch (Exception e)
                    {
                        dbUse.PositionModels.AddOrUpdate(new PositionModel()
                        {
                            PictureId = int.Parse(block[3]),
                            PosX = double.Parse(block[0]),
                            PosY = double.Parse(block[1]),
                            Width = double.Parse(block[2]),
                            Type = block[5]
                        });

                    }
                    
                    
                }
                dbUse.SaveChanges();
            }
            return true;
        }


        public bool DeletePicture(string pictureID)
        {
            using (UserContext dbUse = new UserContext())
            {
                var intID = int.Parse(pictureID);

                var model = dbUse.PicturesModels.FirstOrDefault(t => t.Id == intID);
                dbUse.PicturesModels.Remove(model);
                dbUse.SaveChanges();
                return true;
            }


        }


        public ActionResult UpdatePicture(string Name, int Id)
        {
            SendDbUtility.UpdateImage(Id, Name);

            return RedirectToAction("EditorPattern", "Pattern");
        }


    }
}