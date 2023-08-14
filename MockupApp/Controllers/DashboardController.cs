﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Ajax.Utilities;
using MockupApp.DAO;
using MockupApp.Filters;
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

        [FilterSession(2)]
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }


        [FilterSession(1)]
        public ActionResult Mockups()
        {
            return View();
        }


        public ActionResult Mismockups()
        {
            var ventas = new PayDAO().obtenerComprasUsuario((Session["usuario"] as sp_LoginUsuario_Result).idUsuario);
            ViewBag.ventas = ventas;   
            return View();
        }


        [FilterSession(1)]
        public JsonResult listarMockup()
        {
            var producto = new MockupDAO().listarMockup();
            return Json(new {data = producto}, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [FilterSession(1)]
        public JsonResult buscarMockup(int id) {
            var producto = new MockupDAO().listarMockupPorId(id);
            return Json( producto,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FilterSession(1)]
        public JsonResult actualizarEstadoMockup(int id, bool estado)
        {
            int resultado = new MockupDAO().actualizarEstadoMockup(id, estado);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FilterSession(1)]
        public JsonResult eliminarMockup(int id)
        {
            int resultado = new MockupDAO().eliminarMockup(id);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }


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
            }
            return Json(producto,JsonRequestBehavior.AllowGet);
        }



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
            }
            return Json(producto, JsonRequestBehavior.AllowGet);
        }


    }
}