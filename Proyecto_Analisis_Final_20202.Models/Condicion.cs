using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    class Condicion
    {
        [Key]
        public int ID_Condicion { get; set; }
        //[Required(ErrorMessage = "La condición de la factura es requerida")]
        public string Nombre_Condicion { get; set; }
    }
}
