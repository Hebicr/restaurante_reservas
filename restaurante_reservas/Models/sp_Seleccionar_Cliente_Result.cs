//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace restaurante_reservas.Models
{
    using System;
    
    public partial class sp_Seleccionar_Cliente_Result
    {
        public int id { get; set; }
        public string Cedula { get; set; }
        public System.DateTime FechadeNacimiento { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string TelefonoPrincipal { get; set; }
        public string CorreoELectronico { get; set; }
        public int id_Usuario { get; set; }
        public int id_Rol { get; set; }
    }
}
