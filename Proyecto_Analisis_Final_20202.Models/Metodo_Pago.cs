using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
   public class Metodo_Pago
    {
        [Key]
        public int ID_MetodoPago { get; set; }

        [Required(ErrorMessage = "El nombre del metodo de pago es requerido")]
        [DisplayName("Nombre de método de pago")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del nombre de método de pago es de 25 caracteres")]
        public String Nombre_Metodo { get; set; }
    }
}
