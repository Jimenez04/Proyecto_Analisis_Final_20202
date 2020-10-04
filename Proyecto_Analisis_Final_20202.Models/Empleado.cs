using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Empleado
    { 
        [Key]
        [DisplayName("ID de empleado")]
        public String ID_Empleado { get; set; }

        [Required(ErrorMessage = "La cedula es requerida")]
        [DisplayName("Cedula")]
        [ForeignKey("Persona")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de la cédula es de 25 caracteres")]
        public String Cedula { get; set; }


        [ForeignKey("Profesión")] 
        public int ID_Profesion { get; set; }


        [Required(ErrorMessage = "La cédula jurídica es requerida")]
        [DisplayName("Cedula jurídica")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de la cédula jurídica es de 25 caracteres")]
        public String Cedula_Juridica { get; set; }


        [DisplayName("Salario")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "El salario es requerido")]
        public float Salario { get; set; }


        [DisplayName("Fecha de ingreso")]
        [Required(ErrorMessage = "La fehca de ingreso es requerida")]
        public DateTime Fecha_Ingreso { get; set; }

        

        


    }
}
