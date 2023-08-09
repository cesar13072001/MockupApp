using MockupApp.DAO;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Controllers
{


    public class StoreController : Controller
    {
        readonly mockupEntities db = new mockupEntities();
        // GET: Store
        public ActionResult Index()
        {
            ViewBag.productos = new MockupDAO().listarProductosStore();
            return View();
        }


        public ActionResult Details()
        {
            return View();
        }




        public ActionResult Shopcart()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }



        public ActionResult Salir()
        {
            Session["usuario"] = null;
            return RedirectToAction("", "Store");
        }


        [HttpPost]
        public JsonResult agregarCarrito(int id)
        {
            bool resultado = false;
            if (Session["carrito"] == null)
            {
                List<sp_ListarProductosStore_Result> compras = new List<sp_ListarProductosStore_Result>();
                compras.Add(db.sp_ListarProductosStore().Where(p => p.idProducto == id).First());
                Session["carrito"] = compras;
                resultado = true;
            }
            else
            {
                List<sp_ListarProductosStore_Result> compras = (List<sp_ListarProductosStore_Result>)Session["carrito"];
                int indexExistente = capturarUbicacionProducto(id);
                if (indexExistente == -1)
                {
                    compras.Add(db.sp_ListarProductosStore().Where(p => p.idProducto == id).First());
                    resultado = true;
                }
                else resultado = false;
                Session["carrito"] = compras;
                
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public JsonResult obtenerCarrito()
        {
            List<sp_ListarProductosStore_Result> carrito = new List<sp_ListarProductosStore_Result>();
            if (Session["carrito"] != null)
            {
                carrito = (List<sp_ListarProductosStore_Result>)Session["carrito"];
            }
           return Json(carrito, JsonRequestBehavior.AllowGet);
        }



        public int capturarUbicacionProducto(int id)
        {
            List<sp_ListarProductosStore_Result> compras = (List<sp_ListarProductosStore_Result>)Session["carrito"];
            for (int i = 0; i < compras.Count(); i++)
            {
                if (compras[i].idProducto == id) return i;
            }
            return -1;
        }

    }

}