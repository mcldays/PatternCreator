using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public static class SendDbUtility
    {
        public static bool verifyAutefication(AutModel model)
        {
            using (UserContext dbUse = new UserContext())
            {
                try
                {
                    AutModel user =
                        dbUse.AutModels.FirstOrDefault(t => t.Login == model.Login && t.Password == model.Password);  //Здесь переделать пока костыли

                    if (user == null)
                    {
                        dbUse.AutModels.Add(new AutModel()
                        {
                            Login = model.Login,
                            Password = model.Password




                        });
                        dbUse.SaveChanges();
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




        }


        public static bool sendCompanyModel(string CompanyName)
        {
            using (UserContext dbUse = new UserContext())
            {
                dbUse.CompanyModels.Add(new CompanyModel()
                {
                    CompanyName = CompanyName
                });
                dbUse.SaveChanges();
                return true;
            }
        }


        public static List<CompanyModel> GelAllCompanyList()
        {

            List<CompanyModel> models;

            using (UserContext dbUse = new UserContext())
            {
                models = dbUse.CompanyModels.ToList();
            }

            return models;

        }

        public static bool SendUserToDb(UserModel model)
        {
            using (UserContext dbUse = new UserContext())
            {
                dbUse.UserModels.Add(new UserModel()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    CompanyId = model.CompanyId
                });

                dbUse.SaveChanges();
                return true;
            }
        }


        public static List<object> GroupUsers()
        {
            using (UserContext dbuse = new UserContext())
            {
                //var Group = from p in dbuse.UserModels
                //    join c in dbuse.CompanyModels on p.CompanyId equals c.Id
                //    select new {Name = p.Name, Surname = p.Surname, Company = c.CompanyName};

                var Group = from p in dbuse.CompanyModels
                    join c in dbuse.UserModels on p.Id equals c.CompanyId
                    select new { Name = c.Name, Surname = c.Surname, Company = p.CompanyName };




                var list = new List<object>();
                foreach (var unknown in Group) list.Add(unknown);
                return list;
            }


        }


        public static List<UserModel> GetAllUsers()
        {
            List<UserModel> models;
            using (UserContext dbUse = new UserContext())
            {

               models =  dbUse.UserModels.ToList();
            }

            return models;
        }

        public static List<PictureModel> GetAllTemplates()
        {
            using (UserContext dbUse = new UserContext())
            {
                return dbUse.PicturesModels.ToList();
            }
        }

        public static List<PositionModel> GetAllPositions()
        {
            using (UserContext dbUse = new UserContext())
            {
                return dbUse.PositionModels.ToList();
            }
        }

        public static CompanyModel GetCompanyById(int id)
        {
            CompanyModel model = new CompanyModel();
            using (UserContext dbUse = new UserContext())
            {
                model = dbUse.CompanyModels.FirstOrDefault(t => t.Id == id);
            }

            return model;
        }


        public static bool DeleteCompany(int id)
        {
            try
            {
                using (UserContext dbUse = new UserContext())
                {
                    var model = dbUse.CompanyModels.FirstOrDefault(t => t.Id == id);
                    dbUse.CompanyModels.Remove(model);
                    dbUse.SaveChanges();
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
                using (UserContext dbUse = new UserContext())
                {
                    var model2 = from b in dbUse.UserModels
                        where b.CompanyId == id
                        select b;


                    foreach (var VARIABLE in model2)
                    {
                        dbUse.UserModels.Remove(VARIABLE);
                    }

                    dbUse.SaveChanges();
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
                using (UserContext dbUse = new UserContext())
                {
                    dbUse.UserModels.Remove(dbUse.UserModels.FirstOrDefault(t => t.Id == id));
                    dbUse.SaveChanges();
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
                using (UserContext dbUse = new UserContext())
                {
                    dbUse.UserModels.AddOrUpdate(model);
                    dbUse.SaveChanges();
                }

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
                using (UserContext dbUse = new UserContext())
                {
                    dbUse.CompanyModels.AddOrUpdate(model);
                    dbUse.SaveChanges();

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
            using (UserContext dbUse = new UserContext())
            {
                var modelPicture = dbUse.PicturesModels.FirstOrDefault(t => t.Id == Id);

                modelPicture.Name = Name;

                dbUse.PicturesModels.AddOrUpdate(modelPicture);
                dbUse.SaveChanges();

                return true;
            }
        }

    }
}