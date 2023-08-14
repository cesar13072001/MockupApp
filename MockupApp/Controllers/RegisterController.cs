using MockupApp.DAO;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MockupApp.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult VerificadorCorreo(string email)
        {
            string resultado = string.Empty;
            resultado = new AuthDAO().verificarCorreo(email) == 0 ? "" : "Correo ya registrado";
            if(resultado != "")
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
            
        }

       




        [HttpPost]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            int resultado;
            usuario.idRol = 2;
            usuario.fechaRegistro = DateTime.Now;
            resultado = new AuthDAO().guardarUsuario(usuario);
            if (resultado == 0) Response.StatusCode = (int)HttpStatusCode.BadRequest; 
            return Json(new { resultado = resultado}, JsonRequestBehavior.AllowGet);

        }
    }
}