﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyecto_OIDOCOCINA.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BD_OIDOCOCINAEntities : DbContext
    {
        public BD_OIDOCOCINAEntities()
            : base("name=BD_OIDOCOCINAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GALERIA> GALERIA { get; set; }
        public virtual DbSet<LOCALES> LOCALES { get; set; }
        public virtual DbSet<MESAS> MESAS { get; set; }
        public virtual DbSet<NOTICIAS> NOTICIAS { get; set; }
        public virtual DbSet<RESERVAS> RESERVAS { get; set; }
        public virtual DbSet<USUARIOS> USUARIOS { get; set; }
        public virtual DbSet<COMENTARIOS> COMENTARIOS { get; set; }
    }
}
