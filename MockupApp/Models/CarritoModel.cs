using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockupApp.Models
{
    public class CarritoModel
    {
        public sp_ListarProductosStore_Result Producto { get; set; }
        //public int Cantidad { get; set; }
 
        public CarritoModel(sp_ListarProductosStore_Result producto/*, int cantidad*/) {
            this.Producto = producto;
            //this.Cantidad = cantidad;
        }

    }
}