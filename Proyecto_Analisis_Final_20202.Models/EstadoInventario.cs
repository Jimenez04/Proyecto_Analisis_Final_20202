using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Models
{
    public enum EstadoInventario
    {
        Disponible = 1,

        [Display(Name = "Fuera de servicio")]
        Sin_existencias = 2,
    }
}
