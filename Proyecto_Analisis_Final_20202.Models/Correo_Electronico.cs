using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    class Correo_Electronico
    {
        // No se que hacer porque solo hay una para los 2
        [DisplayName("Cedula")]
        public string Cedula_Cedula_Juridica { get; set; }


        [DisplayName ("Correo eslectrónico")] 
        [Required (ErrorMessage ="El correo electrónico es requerido")]
        public string Correo { get; set; }
    }
}
