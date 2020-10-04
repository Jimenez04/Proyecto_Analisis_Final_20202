using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Estado
    { 
        [Key]
        public int ID_Estado { get; set; }

        [DisplayName("Estado")]
        public String Nombre_Estado { get; set; }
    }
}
