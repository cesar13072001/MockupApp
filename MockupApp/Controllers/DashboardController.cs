using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Ajax.Utilities;
using MockupApp.DAO;
using MockupApp.Filters;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Controllers
{
    public class DashboardController : Controller
    {

        //ActionResult que nos crea la vista de la pagina principal del dashboard
        [FilterSession(2)]
        public ActionResult Index()
        {
            ViewBag.reporte = new PayDAO().obtenerReporteVenta();
            return View();
        }

        //ActionResult que nos crea la vista de la pagina crud de mockups
        [FilterSession(1)]
        public ActionResult Mockups()
        {
            return View();
        }

        //ActionResult que nos crea la vista de la pagina de mockups comprados
        [FilterSession(2)]
        public ActionResult Mismockups()
        {
            List<Venta> ventas = new List<Venta>();
           
            ventas = new PayDAO().obtenerComprasUsuario((Session["usuario"] as sp_LoginUsuario_Result).idUsuario);
           
            ViewBag.ventas = ventas;   
            return View();
        }

        [FilterSession(1)]
        public ActionResult Ventas(DateTime? inicio = null, DateTime? fin = null)
        {
            List<Venta> ventas = new List<Venta>();
            ventas = new PayDAO().obtenerComprasTotales(inicio, fin);
            ViewBag.ventas = ventas;
            return View();
        }

        [FilterSession(1)]
        public ActionResult Usuarios()
        {
            return View();
        }

        [FilterSession(1)]
        public JsonResult listarUsuario()
        {
            var lista = new AuthDAO().listarUsuarios();
            return Json(new { data = lista} ,JsonRequestBehavior.AllowGet);
        }



        //JsonResult para devolver el listado de mockups de la BD
        [FilterSession(1)]
        public JsonResult listarMockup()
        {
            var producto = new MockupDAO().listarMockup();
            return Json(new {data = producto}, JsonRequestBehavior.AllowGet);
        }


        //JsonResult que devuelve un mockup buscado por id
        [HttpPost]
        [FilterSession(1)]
        public JsonResult buscarMockup(int id) {
            var producto = new MockupDAO().listarMockupPorId(id);
            return Json( producto,JsonRequestBehavior.AllowGet);
        }


        //JsonResult que actualiza el estado de activo de un mockup
        [HttpPost]
        [FilterSession(1)]
        public JsonResult actualizarEstadoMockup(int id, bool estado)
        {
            int resultado = new MockupDAO().actualizarEstadoMockup(id, estado);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }

        //JsonResult que elimina un mockup
        [HttpPost]
        [FilterSession(1)]
        public JsonResult eliminarMockup(int id)
        {
            int resultado = new MockupDAO().eliminarMockup(id);
            if(resultado == 0)
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }


        //JsonResult que guarda un mockup en la BD
        [HttpPost]
        [FilterSession(1)]
        public JsonResult guardarMockup(string titulo, string precio, string descuento,
            HttpPostedFileBase psd, HttpPostedFileBase foto)
        {
            HttpPostedFileBase[] files = { psd, foto };
            string[][] resultados = new string[2][];

            List<Contenido> contenidos = new List<Contenido>();
            Producto producto = new Producto();  
            
            try
            {
                producto.titulo = titulo;
                producto.precio = Decimal.Parse(precio);
                producto.estado = true;
                producto.descuento = Int32.Parse(descuento);
                producto.precioDescuento = (producto.descuento == 0) ? 0 : producto.precio - (producto.precio * producto.descuento / 100);
                foreach (var file in files)
                {
                    resultados[Array.IndexOf(files, file)] = new CloudinaryDAO().guardarContenido(file);
                    contenidos.Add(new Contenido { 
                        idCloudinary = resultados[Array.IndexOf(files, file)][0],
                        urlContenido = resultados[Array.IndexOf(files, file)][1],
                        tipo = Array.IndexOf(files, file) == 0 ? false : true,
                    });
                }
                producto = new MockupDAO().guardarMockup(contenidos, producto);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(JsonRequestBehavior.AllowGet);
            }
            return Json(producto,JsonRequestBehavior.AllowGet);
        }


        //JsonResult que edita un mockup y lo guarda en la BD
        [HttpPost]
        [FilterSession(1)]
        public JsonResult editarMockup(string idProducto, string titulo, string precio, string descuento,
            bool estado, HttpPostedFileBase psd = null, HttpPostedFileBase foto = null)
        {
            HttpPostedFileBase[] files = { psd, foto };
            string[][] resultados = new string[2][];

            List<Contenido> contenidos = new List<Contenido>();
            Producto producto = new Producto();

            try
            {
                producto.idProducto = Int32.Parse(idProducto);
                producto.titulo = titulo;
                producto.precio = Decimal.Parse(precio);
                producto.estado = true;
                producto.descuento = Int32.Parse(descuento);
                producto.estado = estado;
                producto.precioDescuento = (producto.descuento == 0) ? 0 :producto.precio - (producto.precio * producto.descuento / 100);
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        resultados[Array.IndexOf(files, file)] = new CloudinaryDAO().guardarContenido(file);
                        contenidos.Add(new Contenido
                        {
                            idCloudinary = resultados[Array.IndexOf(files, file)][0],
                            urlContenido = resultados[Array.IndexOf(files, file)][1],
                            tipo = Array.IndexOf(files, file) == 0 ? false : true,
                        });
                    }
                    else
                    {
                        contenidos.Add(null);
                    }
                }
                producto = new MockupDAO().editarProducto(producto, contenidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(JsonRequestBehavior.AllowGet);
            }
            return Json(producto, JsonRequestBehavior.AllowGet);
        }




       
    }
}