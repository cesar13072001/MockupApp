using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Ajax.Utilities;
using MockupApp.DAO;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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



        public JsonResult listarMockup()
        {
            /*List<Producto> producto = new List<Producto>();
            producto = new MockupDAO().listarMockups();*/

            var producto = new MockupDAO().listarMockup();
            
            

            return Json(new {data = producto}, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult buscarMockup(int id) {
            var producto = new MockupDAO().listarMockupPorId(id);

            return Json(new { producto },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizarEstadoMockup(int id, bool estado)
        {
            int resultado = new MockupDAO().actualizarEstadoMockup(id, estado);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult eliminarMockup(int id)
        {
            int resultado = new MockupDAO().eliminarMockup(id);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarMockup(string titulo, string descripcion, string precio, string descuento,
            HttpPostedFileBase psd, HttpPostedFileBase foto)
        {
            HttpPostedFileBase[] files = { psd, foto };
            string[][] resultados = new string[2][];

            List<Contenido> contenidos = new List<Contenido>();
            Producto producto = new Producto();  
            
            List<Producto> products = new List<Producto>();
            try
            {
                producto.titulo = titulo;
                producto.descripcion = descripcion;
                producto.precio = Decimal.Parse(precio);
                producto.estado = true;
                producto.descuento = Int32.Parse(descuento);
                producto.precioDescuento = producto.precio * producto.descuento / 100;

                foreach (var file in files)
                {
                    resultados[Array.IndexOf(files, file)] = new CloudinaryDAO().guardarContenido(file);
                    contenidos.Add(new Contenido { 
                        idContenido = resultados[Array.IndexOf(files, file)][0],
                        urlContenido = resultados[Array.IndexOf(files, file)][1],
                        tipo = Array.IndexOf(files, file) == 0 ? false : true,
                    });
                }
                producto = new MockupDAO().guardarMockup(contenidos, producto);
                Console.WriteLine(producto);
                products.Add(producto);
                products.Add(producto);
                //salida = producto.ToString();

            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new {products},JsonRequestBehavior.AllowGet);
        }
    }
}