using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
   public  class Empresa
  {
        private Provincia Ubicacion;

        public Empresa()
        {
            Ubicacion = new Provincia();
        }

        [Key]
        [DisplayName("Cedula jurídica")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [MaxLength(15, ErrorMessage = "El tamaño máximo de la cédula jurídica, debe ser de: 25 caracteres")]
        [MinLength(8, ErrorMessage = "El tamaño mínimo de la cédula jurídica  es de 9 digítos")]
        public String Cedula_Juridica { get; set; }


        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [DisplayName("Nombre")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo para el nombre de la empresa es de 4 caracteres")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del nombre de la empresa  es de 25 caracteres")]
        public String  Nombre { get; set; }

       
        [DisplayName("Razon social")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo para el nombre de la empresa es de 4 caracteres")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de la razon social es de 25 caracteres")]
        public String Razon_Social { get; set; }

       
        [DisplayName("Provincia")]
        [Required(ErrorMessage = "La provincia es requerida")]
      
        public int ID_Provincia { get; set; }

        [DisplayName("Cantón")]
        [Required(ErrorMessage = "El cantón es requerido")]
        
        public int ID_Canton { get; set; }

        [DisplayName("Distrito")]
        [Required(ErrorMessage = "El distrito es requerido")]
        public int ID_Distrito { get; set; }

        [DisplayName("Señas exactas")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo del campo señas exactas, debe ser de: 50 caracteres")]
        [MinLength(4, ErrorMessage = "El tamaño mínimo del campo señas exactas, debe ser de: 4 caracteres")]
        [Required(ErrorMessage = "Las señas exactas son requeridas")]
        public String Senas_Exactas { get; set; } 



        [DisplayName("Codigo postal")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del código postal, debe ser de: 25 caracteres")]
        [Required(ErrorMessage = "El código postal es requerido")]
        public String Codigo_Postal { get; set; }

    }
}
