using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Profesion
    { 
        [Key]
       // Este no se si se va a manejar por medio de aqui o es control solo de la BD
        public int ID_Profesion { get; set; }

        [Required(ErrorMessage = "EL nombre de profesión es requerido")]
        [DisplayName("Nombre de profesión")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del nombre de profesión es de 25 caracteres")]
        public String Nombre_Profesion { get; set; }


    }
}
