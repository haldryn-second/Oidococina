//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class COMENTARIOS
    {
        public COMENTARIOS()
        {
            Comentario = "";
        }

        public int Id { get; set; }
        public int Id_Local { get; set; }
        public int Id_Usuario { get; set; }
        public string Comentario { get; set; }
        public int Puntuacion { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Fecha { get; set; }
    
        public virtual LOCALES LOCALES { get; set; }
        public virtual USUARIOS USUARIOS { get; set; }
    }
}
