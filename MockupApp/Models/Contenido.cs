//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MockupApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public partial class Contenido
    {
        public int idContenido { get; set; }
        public string idCloudinary { get; set; }
        public string urlContenido { get; set; }
        public Nullable<bool> tipo { get; set; }
        public Nullable<int> idProducto { get; set; }
        [ScriptIgnore]
        public virtual Producto Producto { get; set; }
    }
}
