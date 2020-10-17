using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey ("Persona")] 
        [Required(ErrorMessage ="La cedula es requerida")]
        [DisplayName ("Cedula")]
        public string Cedula { get; set; }

        [ForeignKey("Factura")]
        [DisplayName("Consecutivo")]
        public string Consecutivo { get; set; }
         

        [DisplayName("Descuento")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Descuento { get; set; }
    }
}
