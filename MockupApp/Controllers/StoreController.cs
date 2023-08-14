using MockupApp.DAO;
using MockupApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MockupApp.Filters;
using System.Configuration;

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

        [FilterSession(2)]
        public async Task<ActionResult> Success()
        {
            string Bearertoken = await obtenerToken();
            string token = Request.QueryString["token"];
            bool status = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearertoken);

                var data = new StringContent("{}", Encoding.UTF8, "application/json");   
                data.Headers.Add("Prefer", "return=representation");
                HttpResponseMessage response = new HttpResponseMessage();
                response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);
                string statuscode = response.StatusCode.ToString();

                if(statuscode != "Created")
                {
                    response.Dispose();
                   
                    HttpResponseMessage responseDetails = await client.GetAsync($"/v2/checkout/orders/{token}");
                    

                    var jsonRespuesta = responseDetails.Content.ReadAsStringAsync().Result;

                    PaypalOrderCapture paypal = JsonConvert.DeserializeObject<PaypalOrderCapture>(jsonRespuesta);


                    DateTime fecha = DateTime.Parse(paypal.UpdateTime.ToString());
                    

                    ViewBag.fecha = fecha.ToString();
                    ViewBag.respuesta = paypal;
                    ViewBag.resultado = jsonRespuesta;
                    ViewBag.productos = paypal.PurchaseUnits[0].Items;
                    ViewBag.id = paypal.Id;
                    ViewBag.total = paypal.PurchaseUnits[0].Amount.Value;


                }


                status = response.IsSuccessStatusCode;

                if (status && statuscode == "Created")
                {
                    var jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    PaypalOrderCapture paypal = JsonConvert.DeserializeObject<PaypalOrderCapture>(jsonRespuesta);

                    
                    DateTime fecha = DateTime.Parse(paypal.UpdateTime.ToString());
                    


                    ViewBag.respuesta = paypal;
                    ViewBag.fecha = fecha.ToString();
                    ViewBag.resultado = jsonRespuesta;
                    ViewBag.productos = paypal.PurchaseUnits[0].Items;
                    ViewBag.id = paypal.Id;
                    ViewBag.total = paypal.PurchaseUnits[0].Amount.Value;

                    
                    Venta venta = new Venta();
                    venta.fechaCompra = fecha;
                    venta.comisionPaypal = Decimal.Parse(paypal.PurchaseUnits[0].Payments.Captures[0].SellerReceivableBreakdown.PaypalFee.Value);
                    venta.idTransaccionPaypal = paypal.Id;
                    venta.totalVenta = decimal.Parse(paypal.PurchaseUnits[0].Amount.Value);
                    venta.idUsuario = (Session["usuario"] as sp_LoginUsuario_Result).idUsuario;

                    List<DetalleVenta> detalles = new List<DetalleVenta>();

                    foreach (var item in paypal.PurchaseUnits[0].Items)
                    {
                        detalles.Add(new DetalleVenta()
                        {
                            idProducto = Int32.Parse(item.Sku),                     
                            subtotal = decimal.Parse(item.UnitAmount.Value),                           
                        });
                    }
                    new PayDAO().guardarPago(venta, detalles);

                    Session["carrito"] = null;
                }         
            }
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


        public JsonResult EliminarProducto(int id)
        {
            List<sp_ListarProductosStore_Result> compras = (List<sp_ListarProductosStore_Result>)Session["carrito"];
            compras.RemoveAt(capturarUbicacionProducto(id));
            return Json(JsonRequestBehavior.AllowGet);
        }



   
        public decimal precioFinal()
        {
            var lista = (List<sp_ListarProductosStore_Result>)Session["carrito"];
            decimal precio = 0;
            foreach (var item in lista)
            {
                precio += (item.precioDescuento != 0) ? (decimal)item.precioDescuento : item.precio;

            }
            return precio;

        }


        public List<Item> obtenerProductosCarrito()
        {
            var lista =  (List<sp_ListarProductosStore_Result>)Session["carrito"];

            List<Item> items = new List<Item>();
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    var costo = (item.precioDescuento != 0) ? item.precioDescuento.ToString() : item.precio.ToString();
                    items.Add(new Item
                    {
                        Name = item.titulo,
                        Quantity = 1,
                        UnitAmount = new ItemTotal()
                        {
                            CurrencyCode = "USD",
                            Value = costo
                        },
                        Sku = item.idProducto.ToString(),
                        ImageUrl = item.urlContenido
                    }) ;
                }
            }
            return items;
        }


        [HttpPost]
        public async Task<string> obtenerToken()
        {
            PaypalAuthorizationResponse authorization = new PaypalAuthorizationResponse();

            using (var client = new HttpClient())
            {
                var idPaypal = ConfigurationManager.AppSettings["clientIdPayPal"];
                var secretPaypal = ConfigurationManager.AppSettings["clientSecretPaypal"];
                var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{idPaypal}:{secretPaypal}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

                var keyValueParis = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string> ( "grant_type", "client_credentials" ),
                };




                HttpResponseMessage response = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token",
                    new FormUrlEncodedContent(keyValueParis));

                var respuesta = response.Content.ReadAsStringAsync().Result;

                authorization = JsonConvert.DeserializeObject<PaypalAuthorizationResponse>(respuesta);
            }
            return authorization.AccessToken;


        }



        [HttpPost]       
        public async Task<JsonResult> Paypal()
        {
            int cantidad = obtenerProductosCarrito().Count();
            bool status = false;
            var respuesta = new PaypalOrderResponse();
            string mensaje = "";
            if (Session["usuario"] == null && cantidad > 0)
            {
                mensaje = "Por favor <a href='/login'>inicie sesión</a>";
                return Json(new { mensaje = mensaje, cantidad = cantidad }, JsonRequestBehavior.AllowGet);
            }
            if(cantidad > 0)
            {

                decimal precio = precioFinal();
                string token = await obtenerToken();

                var url = Request.Url.Scheme + "://" + Request.Url.Authority;

                var successUrl = url + "/store/success";
                var cancelUrl = url + "/?cancel=true";


                var items = obtenerProductosCarrito();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    var orden = new PaypalOrder()
                    {
                        Intent = "CAPTURE",
                        PurchaseUnits = new List<PurchaseUnit> {
                            new PurchaseUnit() {
                                Items = items,
                                Amount = new Amount() {
                                    CurrencyCode = "USD",
                                    Value = precio.ToString(),
                                    Breakdown = new Breakdown(){
                                        ItemTotal = new ItemTotal(){
                                            CurrencyCode = "USD",
                                            Value= precio.ToString()
                                        }
                                    }
                                },

                            }
                        },
                        ApplicationContext = new ApplicationContext()
                        {
                            BrandName = "MockupApp",
                            UserAction = "PAY_NOW", 
                            ReturnUrl = successUrl,
                            CancelUrl = cancelUrl,
                        }
                    };


                    var json = JsonConvert.SerializeObject(orden);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                    status = response.IsSuccessStatusCode;

                    if (status)
                    {
                        respuesta = JsonConvert.DeserializeObject <PaypalOrderResponse>(response.Content.ReadAsStringAsync().Result);
                    }

                }
            }

            return Json(new { status = status, respuesta = respuesta, cantidad = cantidad ,mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }





    }

}