using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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







    }
}