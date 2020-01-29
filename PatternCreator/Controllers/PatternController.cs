using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatternCreator.Models;

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
            return View();
        }

        public ActionResult PrintPage()
        {
            return View();
        }

        public ActionResult EditorPattern()
        {
            return View();
        }
    }
}