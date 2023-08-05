using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["usuario"] == null) return RedirectToAction("", "");
            return View();
        }

        public ActionResult Mockups()
        {
            return View();
        }
    }
}