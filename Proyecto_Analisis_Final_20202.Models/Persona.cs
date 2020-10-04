using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Persona
    {
        [Key]
        [DisplayName("Identificación")]
        [Required(ErrorMessage = "La identificación es requerida")]
        [MaxLength(9, ErrorMessage = "El tamaño máximo de la identificacion es de 9 digítos")]
        [MinLength(9, ErrorMessage = "El tamaño mínimo de la identificacion es de 9 digítos")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public String  Cedula { get; set; }


        [DisplayName("Primer Nombre")]
        [Required(ErrorMessage = "Su primer nombre es requerido")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de su primer nombre debe ser de: 25 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo de su primer nombre debe ser de: 4 caracteres")]
        public String Nombre1 { get; set; }


        [DisplayName("Segundo Nombre")]
        [Required(ErrorMessage = "Su segundo nombre es requerido")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de su segundo nombre debe ser de: 25 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo de su segundo nombre debe ser de: 4 caracteres")]
        public String Nombre2 { get; set; }


        [DisplayName("Primer Apellido")]
        [Required(ErrorMessage = "Su primer apellido es requerido")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de su primer apellido debe ser de: 25 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo de su primer apellido debe ser de: 4 caracteres")]
        public String Apellido1 { get; set; }


        [DisplayName("Segundo Apellido")]
        [Required(ErrorMessage = "Su segundo apellido es requerido")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de su segundo apellido debe ser de: 25 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo de su segundo apellido debe ser de: 4 caracteres")]
        public String Apellido2 { get; set; }



        [DisplayName("Sexo")]
        [ForeignKey("Sexo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int ID_Sexo { get; set; }



        [DisplayName("Provincia")]
        [Required(ErrorMessage = "La provincia es requerida")]
        [ForeignKey("Provincia")]
        public int ID_Provincia { get; set; }


        [DisplayName("Cantón")]
        [Required(ErrorMessage = "El cantón es requerido")]
        [ForeignKey("Canton")]
        public int ID_Canton { get; set; }


        [DisplayName("Distrito")]
        [Required(ErrorMessage = "El distrito es requerido")]
        [ForeignKey("Distrito")]
        public int ID_Distrito { get; set; }


        [DisplayName("Señas Exactas")]
        [Required(ErrorMessage = "El campo: señas exactas es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo del campo señas exactas, debe ser de: 50 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo del campo señas exactas, debe ser de: 4 caracteres")]
        public String SenasExactas { get; set; }


        [DisplayName("Código Postal")]
        [Required(ErrorMessage = "El campo: señas exactas es requerido")]
        public string Codigo_Postal { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo debe ser de: 12 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo debe ser de: 8 caracteres")]
        [Phone(ErrorMessage = "Verifique su número teléfonico")]
        [NotMapped]
        public string  NumeroTelefonico { get; set; }
        [NotMapped]
        [EmailAddress(ErrorMessage = "Verifique su correo electrónico")]
        [DisplayName("Correo Electrónico")]
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo del correo electrónico, debe ser de: 20 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo del correo electrónico, debe ser de: 8 caracteres")]
        public string CorreoElectronico { get; set; }

    }
}
