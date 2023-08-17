using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MockupApp.Models;

namespace MockupApp.DAO
{
    public class MockupDAO
    {
        readonly mockupEntities db = new mockupEntities();

        public Producto guardarMockup(List<Contenido> contenidos, Producto producto)
        {          
            try
            {
                producto.Contenido = (from contenido in contenidos
                                      select new Contenido
                                      {
                                          idCloudinary = contenido.idCloudinary,
                                          urlContenido = contenido.urlContenido,
                                          tipo = contenido.tipo,
                                      }).ToList();
                db.Producto.Add(producto);
                db.SaveChanges();          
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                producto = null;                
            }
            return producto;
        }


        public Producto editarProducto(Producto producto, List<Contenido> contenidos = null)
        {
            try
            {
                var product = db.Producto.Find(producto.idProducto);
                if (producto.titulo != product.titulo) product.titulo = producto.titulo;
                if (producto.precio != product.precio) product.precio = producto.precio;
                if (producto.descuento != product.descuento) product.descuento = producto.descuento;
                if (producto.precioDescuento != product.precioDescuento) product.precioDescuento = producto.precioDescuento;
                if (producto.estado != product.estado) product.estado = producto.estado;

                if (contenidos.Count != 0)
                {
                    if (contenidos[0] != null)
                    {
                        var psd = db.Contenido.Where(c => c.idProducto == producto.idProducto && c.tipo == false).First();
                        new CloudinaryDAO().eliminarContenido(psd.idCloudinary);
                        psd.idCloudinary = contenidos[0].idCloudinary;
                        psd.urlContenido = contenidos[0].urlContenido;
                    }

                    if (contenidos[1] != null)
                    {
                        var foto = db.Contenido.Where(c => c.idProducto == producto.idProducto && c.tipo == true).First();
                        new CloudinaryDAO().eliminarContenido(foto.idCloudinary);
                        foto.idCloudinary = contenidos[1].idCloudinary;
                        foto.urlContenido = contenidos[1].urlContenido;
                    }
                }
                db.SaveChanges();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                producto = null;
            }
            return producto;
        }





        public IEnumerable<sp_ListarProductos_Result> listarMockup()
        {
            List<sp_ListarProductos_Result> listado = new List<sp_ListarProductos_Result> ();
            try
            {
                listado = db.sp_ListarProductos().ToList();
            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listado;
        }


        public int actualizarEstadoMockup(int id, bool estado)
        {
            int resultado = 0;
            try
            {
                Producto producto = db.Producto.Find(id);
                producto.estado = estado;
                resultado = db.SaveChanges();

            }catch(Exception ex)
            {
                Console.WriteLine (ex.Message);
            }
            return resultado;
        }

        public int eliminarMockup(int id)
        {
            int resultado = 0;
            try
            {
                var producto = db.Producto.Find(id);
                var contenido = db.Contenido.Where(x => x.idProducto == id);
                db.Contenido.RemoveRange(contenido);
                db.Producto.Remove(producto);
                resultado = db.SaveChanges();
                foreach(var item in contenido)
                {
                    new CloudinaryDAO().eliminarContenido(item.idCloudinary);
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine ($"{ex.Message}");
            }
            return resultado;
        }



        public IEnumerable<Producto> listarMockupPorId(int id)
        {
            List<Producto> productos = new List<Producto>();
            try
            {
                var lista = db.Producto.Where(x => x.idProducto == id).ToList();
                foreach (var item in lista)
                {
                    var contenido = new List<Contenido>();                   
                    foreach (var item2 in item.Contenido)
                    {
                        contenido.Add(new Contenido
                        {
                            idContenido = item2.idContenido,
                            idCloudinary = item2.idCloudinary,
                            tipo = item2.tipo,
                            idProducto = item2.idProducto,
                            urlContenido = item2.urlContenido,
                        });
                    }
                  
                    productos.Add(new Producto
                    {
                        idProducto = item.idProducto,
                        titulo = item.titulo,
                        precio = item.precio,
                        estado = item.estado,
                        descuento = item.descuento,
                        precioDescuento = item.precioDescuento,
                        Contenido = contenido,              
                    });
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return productos;
        }



        public IEnumerable<sp_ListarProductosStore_Result> listarProductosStore()
        {
            List<sp_ListarProductosStore_Result> productos = new List<sp_ListarProductosStore_Result>();
            try
            {
                var resultado = from d in db.sp_ListarProductosStore() select d;
                productos.AddRange(resultado);

            }catch(Exception ex) { 
            
            Console.WriteLine(ex.Message);
            }

            return productos;
        }

        public string obtenerArchivoMockup(int idProducto)
        {
            string archivo = string.Empty;
            try
            {
                var consulta = db.Contenido.Where(c => c.idProducto == idProducto && c.tipo == false).First();

                archivo = consulta.urlContenido;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return archivo;
        }

    }
}