using MockupApp.DAO;
using MockupApp.Filters;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace MockupApp.Controllers
{
    public class RegisterController : Controller
    {
        //ActionResult para la creacion de la vista de registro
        // GET: Register
        [FilterAuth]
        public ActionResult Index()
        {
            return View();
        }


        //JsonResult para verificar el correo del usuario si se encuentra registrado en BD
        [HttpPost]
        [FilterAuth]
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

       



        //JsonResult para guardar un usuario a la BD
        [HttpPost]
        [FilterAuth]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            int resultado;
            usuario.idRol = 2;
            byte[] hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(usuario.contrasenia));

            string password = Convert.ToBase64String(hash);

            usuario.contrasenia = password;

            usuario.fechaRegistro = DateTime.Now;
            resultado = new AuthDAO().guardarUsuario(usuario);
            if (resultado == 0) Response.StatusCode = (int)HttpStatusCode.BadRequest; 
            return Json(new { resultado = resultado}, JsonRequestBehavior.AllowGet);

        }
    }
}