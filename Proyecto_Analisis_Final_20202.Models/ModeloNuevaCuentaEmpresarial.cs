using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class ModeloNuevaCuentaEmpresarial
    {
        [DisplayName("Cédula Jurídica")]
        [Required(ErrorMessage = "La Cédula Jurídica es requerida")]
        [MaxLength(10, ErrorMessage = "El tamaño máximo de la Cédula Jurídica es de 10 digítos")]
        [MinLength(10, ErrorMessage = "El tamaño mínimo de la Cédula Jurídica es de 10 digítos")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public String Cedula { get; set; }

        [DisplayName("Nombre Completo")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del nombre debe ser de: 25 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo del nombre debe ser de: 4 caracteres")]
        public string NombreCompleto { get; set; }

        

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo debe ser de: 12 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo debe ser de: 8 caracteres")]
        [Phone(ErrorMessage = "Verifique su número teléfonico")]
        public string NumeroTelefonico { get; set; }

        [EmailAddress(ErrorMessage = "Verifique su correo electrónico")]
        [DisplayName("Correo Electrónico")]
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo del correo electrónico, debe ser de: 20 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo del correo electrónico, debe ser de: 8 caracteres")]
        public string CorreoElectronico { get; set; }
    }
}
