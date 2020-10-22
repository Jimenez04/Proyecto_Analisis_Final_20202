﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public class DetalleFactura
    {
        public int ID { get; set; }

       
        public string Consecutivo { get; set; }

     
        public string Codigo_Producto { get; set; }


      
        public double Precio_Unidad { get; set; }
       
        public string Nombre_Articulo { get; set; }

      
        public string SubTotal { get; set; }
      
        public int Cantidad { get; set; }

       
        public int Descuento { get; set; }

       
        public double Total { get; set; }

       
        public int Impuesto_Producto { get; set; }
    }
}
