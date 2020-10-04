using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    class Categoria
    {
        [Key]
        public int ID_Categoria { get; set; }

        [DisplayName("Nombre de categoria")]
        public String Nombre_Categoria { get; set; }

    }
}
