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







    }
}