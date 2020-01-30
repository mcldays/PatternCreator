using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
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
    }
}