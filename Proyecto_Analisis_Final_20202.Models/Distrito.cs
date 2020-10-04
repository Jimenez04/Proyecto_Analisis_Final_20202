using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Distrito
    {
        [Key]
        public int ID_Distrito { get; set; }
       // [Key]
        [ForeignKey("Canton")]
        public int ID_Canton { get; set; }
        //[Key]
        [ForeignKey("Provincia")]
        public int ID_Provincia { get; set; }

        public String Nombre { get; set; }

    }
}
