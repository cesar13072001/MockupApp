using MockupApp.DAO;
using MockupApp.Filters;
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
        //ActionResult que crea la vista de login
        [FilterAuth]
        public ActionResult Index()
        {
            return View();
        }


        //JsonResult que hace el logeo al sistema consultando a la BD
        [HttpPost]
        [FilterAuth]
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