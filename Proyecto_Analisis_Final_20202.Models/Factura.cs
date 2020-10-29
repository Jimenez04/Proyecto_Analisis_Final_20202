using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Factura
    {
        public DetalleFactura detallefactura = new DetalleFactura();

        [Key]
        [DisplayName("Consecutivo")]
        public string Consecutivo { get; set; }


        [ForeignKey("Empresa")]
        [MaxLength(15, ErrorMessage = "El tamaño máximo de la cédula jurídica, debe ser de: 25 caracteres")]
        [MinLength(8, ErrorMessage = "El tamaño mínimo de la cédula jurídica  es de 9 digítos")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [DisplayName("Cedula Jurídica")]
        public string Cedula_Juridica { get; set; }

        [ForeignKey("Condicion")]
        public int ID_Condicion { get; set; }


        [DisplayName("Plazo de crédito")]
        public int Plazo_Credito { get; set; }



        [ForeignKey("Metodo_Pago")]
        public int ID_MetodoPago { get; set; }


        [Required(ErrorMessage = "El sub total es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [DisplayName("Sub total")]
        public double SubTotal { get; set; }


        
        [DisplayName("Descuento")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Descuento { get; set; }


        [Required(ErrorMessage = "El IVA es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [DisplayName("IVA")]
        public int IVA { get; set; }


        [Required(ErrorMessage = "El Total es requerido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [DisplayName("Total")]
        public double Total { get; set; }


        [DisplayName("Fecha de emisión")]
        public DateTime Fecha_Emision { get; set; }


        [DisplayName("Clave de factura")]
        public string Clave { get; set; }


        [DisplayName("Código de actividad")]
        public int Codigo_Actividad { get; set; }
    }
}
