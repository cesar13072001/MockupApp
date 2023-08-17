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


        

        public List<Venta> obtenerComprasTotales(DateTime? fechaInicio, DateTime? fechaFinal)
        {
            List<Venta> venta = new List<Venta>();
            try
            {
                if(fechaInicio == null && fechaFinal == null)
                {
                    venta = db.Venta.ToList();
                }
                else if(fechaFinal == null)
                {
                             
                    venta =db.Venta.Where(v => ((DateTime)v.fechaCompra).ToString().Substring(0,10) == fechaInicio.ToString().Substring(0,10)).ToList();    
                }
                else if(fechaInicio != null && fechaFinal != null)
                {
                    var fechaF = ((DateTime)fechaFinal).AddDays(1);
                    venta = db.Venta.Where(v => v.fechaCompra >= fechaInicio && v.fechaCompra <= fechaF ).ToList();
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return venta;
        }

        public sp_ReportesVenta_Result obtenerReporteVenta()
        {
            sp_ReportesVenta_Result reporte = new sp_ReportesVenta_Result();
            try
            {
                reporte = db.sp_ReportesVenta().First();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return reporte;
        }

    }
}