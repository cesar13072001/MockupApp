using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockupApp.DAO
{
    public class PayDAO
    {
        readonly mockupEntities db = new mockupEntities();

        public void guardarPago(Venta venta,List<DetalleVenta> detalles)
        {
            try
            {
                venta.DetalleVenta = detalles.ToList();
                db.Venta.Add(venta);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());               
            }
        }


        public List<Venta> obtenerComprasUsuario(int idUsuario)
        {
            List<Venta> ventas = new List<Venta>();
            try
            {
                ventas = db.Venta.Where(v => v.idUsuario == idUsuario).ToList();    

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return ventas;
        }


        public List<Venta> obtenerCompraUsuario(int idUsuario)
        {
            List<Venta> venta = new List<Venta>();
            try
            {
                venta = db.Venta.Where(v => v.idUsuario == idUsuario).ToList();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return venta;
        }

    }
}