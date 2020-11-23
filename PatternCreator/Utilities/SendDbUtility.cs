using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class SendDbUtility
    {
        
        public static bool verifyAutefication(AutModelViewModel model)
        {
                //try
                //{
                    using (UserContext db = new UserContext())
                    {
                        AutModel user =
                            db.AutModels.FirstOrDefault(t => t.Login == model.Login && t.Password == model.Password);  //Здесь переделать пока костыли

                        if (user == null)
                        {
                            //db.AutModels.Add(new AutModel()
                            //{
                            //    Login = model.Login,
                            //    Password = model.Password
                            //});
                            //db.SaveChanges();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                //}
                //catch (Exception e)
                //{
                //    return false;
                //}
           
        }


        public static bool sendCompanyModel(CreateCompanyModel Company)
        {
            using (UserContext db = new UserContext())
            {
                db.CompanyModels.Add(new CompanyModel()
                {
                    CompanyName = Company.CompanyName
                });
                db.SaveChanges();
                return true;
            }
               
           
        }


        public static CompanyViewModel[] GelAllCompanyList()
        {
            using (UserContext db = new UserContext())
            {
                return db.CompanyModels.ToArray().Select(t=>new CompanyViewModel
                {
                    CompanyName = t.CompanyName,
                    CompanyId = t.CompanyId,
                    UserViewModels = t.UserModels.Select(u=>new UserModelViewModel(u))
                }).ToArray();  
            }
        }

        public static bool SendUserToDb(UserModel model)
        {
            using (UserContext db = new UserContext())
            {
                db.UserModels.AddOrUpdate(model);
                db.SaveChanges();
                return true;
            }
           
        }


        public static List<object> GroupUsers()
        {
            
                //var Group = from p in db.UserModels
                //    join c in db.CompanyModels on p.CompanyId equals c.Id
                //    select new {Name = p.Name, Surname = p.Surname, Company = c.CompanyName};

            using (UserContext db = new UserContext())
            {
                var Group = from p in db.CompanyModels
                    join c in db.UserModels on p.CompanyId equals c.CompanyId
                    select new { Name = c.Name, Surname = c.Surname, Company = p.CompanyName };

                var list = new List<object>();
                foreach (var unknown in Group) list.Add(unknown);
                return list;
            }
            
        }


        public static List<UserModel> GetAllUsers()
        {
            using (UserContext db = new UserContext())
            {
                List<UserModel> models;
                models =  db.UserModels.ToList();
            

                return models;
            }
        }

        public static List<PictureModel> GetAllTemplates()
        {

            using (UserContext db = new UserContext())
            {
                return db.PicturesModels.ToList();
            }
           
        }

        public static List<PositionModel> GetAllPositions()
        {
            using (UserContext db = new UserContext())
            {
                return db.PositionModels.ToList();
            }
            
        }

        public static CompanyModel GetCompanyById(int id)
        {
            using (UserContext db = new UserContext())
            {
                CompanyModel model = new CompanyModel();
            
                model = db.CompanyModels.FirstOrDefault(t => t.CompanyId == id);

                return model;
            }
        }


        public static bool DeleteCompany(int id)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    var model = db.CompanyModels.Find(id);
                    if (model==null)
                        return false;
                    db.CompanyModels.Remove(model);
                    db.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public static bool DeleteAllUsersInCompany(int id)
        {
            try
            {

                using (UserContext db = new UserContext())
                {
                    var model2 = from b in db.UserModels
                        where b.CompanyId == id
                        select b;

                    foreach (var VARIABLE in model2)
                    {
                        db.UserModels.Remove(VARIABLE);
                    }

                    db.SaveChanges();
                    return true;
                }

                
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool DeleteUserById(int id)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    UserModel model = db.UserModels.Find(id);
                    if (model == null)
                        return false;
                    db.UserModels.Remove(model);
                    db.SaveChanges();
                    return true;
                }
               
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool UpdateUser(UserModel model)
        {
            try
            {

                using (UserContext db = new UserContext())
                {
                    db.UserModels.AddOrUpdate(model);
                    db.SaveChanges();
                
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool SaveCompany(CompanyModel model)
        {
            try
            {

                using (UserContext db = new UserContext())
                {
                    db.CompanyModels.AddOrUpdate(model);
                    db.SaveChanges();

                    return true;
                }
                
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool UpdateImage(int Id, string Name)
        {

            using (UserContext db = new UserContext())
            {
                var modelPicture = db.PicturesModels.Find(Id);
                if (modelPicture==null)
                    return false;

                modelPicture.Name = Name;

                db.Entry(modelPicture).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            
        }



        public static List<PictureModel> GetAllPictures()
        {
            using (UserContext db = new UserContext())
            {
                var models = db.PicturesModels.ToList();
                return models;
            }
        }

    }
}