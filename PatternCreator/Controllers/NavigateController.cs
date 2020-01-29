using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatternCreator.Controllers
{
    public class NavigateController : Controller
    {
        // GET: Aut
        public ActionResult NavigateHome()
        {
            return RedirectToAction("Home", "Pattern");
        }

        public ActionResult NavigatePrintPage()
        {
            return RedirectToAction("PrintPage", "Pattern");
        }

        public ActionResult NavigateEditorPattern()
        {
            return RedirectToAction("EditorPattern", "Pattern");
        }

    }
}