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
    
    public partial class Usuario
    {
        public int idUsuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string contrasenia { get; set; }
        public int idRol { get; set; }
    
        public virtual Rol Rol { get; set; }

       
    }
}