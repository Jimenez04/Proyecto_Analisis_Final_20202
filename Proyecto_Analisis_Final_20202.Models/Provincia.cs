using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;


namespace Proyecto_Analisis_Final_20202.Models
{
   public class Provincia
    {
        public Canton canton = new Canton();

        [Key]
        public int ID_Provincia { get; set; }

        [DisplayName("Nombre Provincia")]
        //[Required(ErrorMessage = "Seleccione Provincia")]
        public string Nombre_Provincia { get; set; }


      
    }
}
