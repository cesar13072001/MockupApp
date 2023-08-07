﻿using System;
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
                                          idContenido = contenido.idContenido,
                                          urlContenido = contenido.urlContenido,
                                          tipo = contenido.tipo,
                                      }).ToList();

                db.Producto.Add(producto);

                db.SaveChanges();
                
            }

            catch (DbEntityValidationException e)
            {
                producto = null;
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            return producto;
        }

        public List<Producto> listarMockups()
        {
            List<Producto> producto = new List<Producto>();
            
            try
            {
                /*from d in db.usp_listar_medicamentos_db()
                select d;*/

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

    }
}