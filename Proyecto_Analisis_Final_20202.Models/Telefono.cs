using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Analisis_Final_20202.Models
{
    class Telefono
    {
        public String Cedula_CedulaJuridica { get; set; }
        [DisplayName("Número Telefónico")]
        [Required(ErrorMessage = "El número telefónico es requerido")]
        [MaxLength(9, ErrorMessage = "El tamaño máximo de la identificacion es de 25 digítos")]
        [MinLength(9, ErrorMessage = "El tamaño mínimo de la identificacion es de 6 digítos")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public String Numero { get; set; }
    }
}
