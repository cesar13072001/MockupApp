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
                if (producto.descripcion != product.descripcion) product.descripcion = producto.descripcion;
                if (producto.precio != product.precio) product.precio = producto.precio;
                if (producto.descuento != product.descuento) product.descuento = producto.descuento;
                if (producto.precioDescuento != product.precioDescuento) product.precioDescuento = producto.precioDescuento;
                if (producto.estado != product.estado) product.estado = producto.estado;

                if (contenidos.Count != 0)
                {
                    if (contenidos[0] != null)
                    {
                        var psd = db.Contenido.Where(c => c.idProducto == producto.idProducto && c.tipo == false).First();
                        psd.idCloudinary = contenidos[0].idCloudinary;
                        psd.urlContenido = contenidos[0].urlContenido;
                    }

                    if (contenidos[1] != null)
                    {
                        var foto = db.Contenido.Where(c => c.idProducto == producto.idProducto && c.tipo == true).First();
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




        public List<Producto> listarMockups()
        {
            List<Producto> producto = new List<Producto>();
            
            try
            {
              

                var lista = from p in db.Producto select p;
                foreach(var item in lista)
                {
                    var contenido = new List<Contenido>();
                    var cupon = new List<Cupon>();

                    foreach(var item2 in item.Contenido)
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

                    foreach(var item3 in item.Cupon)
                    {
                        cupon.Add(new Cupon
                        {
                            idCupon = item3.idCupon,
                            idProducto=item3.idProducto,
                            cantidad = item3.cantidad,
                            estadoCupon = item3.estadoCupon,
                            valorDescuento = item3.valorDescuento,
                        });
                    }


                    producto.Add(new Producto
                    {
                        idProducto = item.idProducto,
                        titulo = item.titulo,
                        descripcion = item.descripcion,
                        precio = item.precio,
                        estado = item.estado,
                        descuento = item.descuento,
                        precioDescuento = item.precioDescuento,            
                        Contenido = contenido,
                        Cupon = cupon,
                    });
                    
                }
                           
             
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var cupon = db.Cupon.Where(x => x.idProducto == id);
                db.Contenido.RemoveRange(contenido);
                db.Cupon.RemoveRange(cupon);
                db.Producto.Remove(producto);
                resultado = db.SaveChanges();
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
                    var cupon = new List<Cupon>();

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

                    foreach (var item3 in item.Cupon)
                    {
                        cupon.Add(new Cupon
                        {
                            idCupon = item3.idCupon,
                            idProducto = item3.idProducto,
                            cantidad = item3.cantidad,
                            estadoCupon = item3.estadoCupon,
                            valorDescuento = item3.valorDescuento,
                        });
                    }
                    productos.Add(new Producto
                    {
                        idProducto = item.idProducto,
                        titulo = item.titulo,
                        descripcion = item.descripcion,
                        precio = item.precio,
                        estado = item.estado,
                        descuento = item.descuento,
                        precioDescuento = item.precioDescuento,
                        Contenido = contenido,
                        Cupon = cupon,
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

    }
}