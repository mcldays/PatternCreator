using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class SendDbUtility
    {
        private static UserContext db = new UserContext();
        public static bool verifyAutefication(AutModelViewModel model)
        {
                try
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
                catch (Exception e)
                {
                    return false;
                }
           
        }


        public static bool sendCompanyModel(CreateCompanyModel Company)
        {
            
                db.CompanyModels.Add(new CompanyModel()
                {
                    CompanyName = Company.CompanyName
                });
                db.SaveChanges();
                return true;
           
        }


        public static CompanyViewModel[] GelAllCompanyList()
        {
            return db.CompanyModels.ToArray().Select(t=>new CompanyViewModel
            {
                CompanyName = t.CompanyName,
                CompanyId = t.CompanyId,
                UserViewModels = t.UserModels.Select(u=>new UserModelViewModel(u))
            }).ToArray();  
        }

        public static bool SendUserToDb(UserModel model)
        {
                db.UserModels.AddOrUpdate(model);
                db.SaveChanges();
                return true;
           
        }


        public static List<object> GroupUsers()
        {
            
                //var Group = from p in db.UserModels
                //    join c in db.CompanyModels on p.CompanyId equals c.Id
                //    select new {Name = p.Name, Surname = p.Surname, Company = c.CompanyName};

                var Group = from p in db.CompanyModels
                    join c in db.UserModels on p.CompanyId equals c.CompanyId
                    select new { Name = c.Name, Surname = c.Surname, Company = p.CompanyName };

                var list = new List<object>();
                foreach (var unknown in Group) list.Add(unknown);
                return list;
            
        }


        public static List<UserModel> GetAllUsers()
        {
            List<UserModel> models;
               models =  db.UserModels.ToList();
            

            return models;
        }

        public static List<PictureModel> GetAllTemplates()
        {
            
                return db.PicturesModels.ToList();
           
        }

        public static List<PositionModel> GetAllPositions()
        {
                return db.PositionModels.ToList();
            
        }

        public static CompanyModel GetCompanyById(int id)
        {
            CompanyModel model = new CompanyModel();
            
                model = db.CompanyModels.FirstOrDefault(t => t.CompanyId == id);
            

            return model;
        }


        public static bool DeleteCompany(int id)
        {
            try
            {
                    var model = db.CompanyModels.Find(id);
                    if (model==null)
                        return false;
                    db.CompanyModels.Remove(model);
                    db.SaveChanges();
                    return true;
                
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
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool DeleteUserById(int id)
        {
            try
            {
                UserModel model = db.UserModels.Find(id);
                if (model == null)
                    return false;
                db.UserModels.Remove(model);
                db.SaveChanges();
                return true;
               
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
                
                    db.UserModels.AddOrUpdate(model);
                    db.SaveChanges();
                
                return true;
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
                
                    db.CompanyModels.AddOrUpdate(model);
                    db.SaveChanges();

                    return true;
                
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool UpdateImage(int Id, string Name)
        {
            
                var modelPicture = db.PicturesModels.FirstOrDefault(t => t.Id == Id);

                modelPicture.Name = Name;

                db.PicturesModels.AddOrUpdate(modelPicture);
                db.SaveChanges();

                return true;
            
        }



        public static List<PictureModel> GetAllPictures()
        {
                var models = db.PicturesModels.ToList();
                return models;
        }

    }
}