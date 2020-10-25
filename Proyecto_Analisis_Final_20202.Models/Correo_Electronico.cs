using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Correo_Electronico
    {
        // No se que hacer porque solo hay una para los 2 

        [ForeignKey("Persona")]
        [Key]
        [DisplayName("Cedula")]
        public string Cedula { get; set; }


        [DisplayName ("Correo Electrónico")] 
        [Required (ErrorMessage ="El correo electrónico es requerido")]
        public string Correo { get; set; }
    }
}
