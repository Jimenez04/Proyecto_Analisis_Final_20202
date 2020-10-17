using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class DetalleFactura
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Empresa")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [DisplayName("Consecutivo")]
        public string Consecutivo { get; set; }

        [ForeignKey("Inventario")]
        [DisplayName("Código")]
        public string Codigo_Producto { get; set; }


        [DisplayName("Precio Unidad")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "El precio de la unidad es requerido")]
        public double Precio_Unidad { get; set; }
        //
        [DisplayName("Nombre")]
        [NotMapped]
        public string Nombre_Articulo { get; set; }

        [DisplayName("SubTotal")]
        [NotMapped]
        public string SubTotal { get; set; }
        //
        [DisplayName("Cantidad")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "La cantidad es requerida")]
        public int Cantidad { get; set; }

        [DisplayName("Descuento")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Descuento { get; set; }

        [DisplayName("Total")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "El total es requerido")]
        public double Total { get; set; }

        [DisplayName("Impuesto")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Impuesto_Producto { get; set; }
    }
}
