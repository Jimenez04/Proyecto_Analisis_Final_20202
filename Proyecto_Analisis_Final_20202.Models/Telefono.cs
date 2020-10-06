using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Telefono
    {
        [ForeignKey("Persona")]
        [Key]
        public String Cedula { get; set; }
        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo debe ser de: 12 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo debe ser de: 8 caracteres")]
        [Phone(ErrorMessage = "Verifique su número teléfonico")]
        public String Numero { get; set; }
    }
}
