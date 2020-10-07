using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class Inventario
    { 
        [Key]
        //[Required(ErrorMessage = "El codigo de producto es requerido")]
        [DisplayName("Código de Producto ")]
       // [MaxLength(25, ErrorMessage = "El tamaño máximo del codigo de producto es de 25 caracteres")]
        public String Codigo_Prodcuto { get; set; }

        [Required(ErrorMessage = "La nombre del producto es requerido")]
        [DisplayName("Nombre del producto")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo del nombre del producto es de 25 caracteres")]
        public String Nombre { get; set;  }


        [Required(ErrorMessage = "El precio de venta es requerido")]
        [DisplayName("Precio de venta")]
        public Double Precio_Venta { get; set; }


        [Required(ErrorMessage = "El precio de compra es requerido")]
        [DisplayName("Precio de compra")]
        public Double Precio_Compra { get; set; }


        [Required(ErrorMessage = "La descripción es requerida")]
        [DisplayName("Descripción")]
        [MaxLength(25, ErrorMessage = "El tamaño máximo de la descripción es de 25 caracteres")]
        public String Descripcion { get; set; }

        [DisplayName("Categoria")]
        public int ID_Categoria { get; set; }


        [DisplayName("Cantidad Disponible")]
        public int Cantidad_Disponible { get; set; }

        [ForeignKey("Estado")]
        [DisplayName("Estado")]
        public EstadoInventario ID_Estado { get; set; }
    }
}
