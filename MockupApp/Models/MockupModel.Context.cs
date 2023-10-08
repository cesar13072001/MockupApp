﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class mockupEntities : DbContext
    {
        public mockupEntities()
            : base("name=mockupEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contenido> Contenido { get; set; }
        public virtual DbSet<DetalleVenta> DetalleVenta { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
    
        public virtual ObjectResult<sp_ListarProductos_Result> sp_ListarProductos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ListarProductos_Result>("sp_ListarProductos");
        }
    
        public virtual ObjectResult<sp_ListarProductosStore_Result> sp_ListarProductosStore()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ListarProductosStore_Result>("sp_ListarProductosStore");
        }
    
        public virtual ObjectResult<sp_LoginUsuario_Result> sp_LoginUsuario(string correo, string contrasenia)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("correo", correo) :
                new ObjectParameter("correo", typeof(string));
    
            var contraseniaParameter = contrasenia != null ?
                new ObjectParameter("contrasenia", contrasenia) :
                new ObjectParameter("contrasenia", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_LoginUsuario_Result>("sp_LoginUsuario", correoParameter, contraseniaParameter);
        }
    
        public virtual ObjectResult<sp_ReportesVenta_Result> sp_ReportesVenta()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ReportesVenta_Result>("sp_ReportesVenta");
        }
    }
}
