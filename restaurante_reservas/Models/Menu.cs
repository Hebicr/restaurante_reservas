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
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int id { get; set; }
        public string platillo { get; set; }
        public string descripcion { get; set; }
        public string img { get; set; }
        public int id_Categoria { get; set; }
        public int id_Estado { get; set; }
    
        public virtual categoria_menu categoria_menu { get; set; }
        public virtual Estados Estados { get; set; }
    }
}
