using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Sexo
    {  
        [Key]
        public int ID_Sexo { get; set; }

        public String Nombre_Sexo { get; set; }

    }
}
