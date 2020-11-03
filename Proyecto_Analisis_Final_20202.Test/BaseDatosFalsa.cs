using Microsoft.EntityFrameworkCore;
using Proyecto_Analisis_Final_20202.DA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Analisis_Final_20202.Test
{
   public class BaseDatosFalsa
    {
        protected ContextoBaseDeDatos CrearContexto(string nombre)
        {
            var opciones = new DbContextOptionsBuilder<ContextoBaseDeDatos>().UseInMemoryDatabase(nombre).Options;
            var dbContext = new ContextoBaseDeDatos(opciones);
            return dbContext;
        }
    }
}
