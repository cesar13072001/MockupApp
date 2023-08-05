using MockupApp.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult LogeoUsuario(string email, string password)
        {
            var usuario = new AuthDAO().logeo(email, password);
            if (usuario.idUsuario == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                Session["usuario"] = usuario;
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        


    }
}