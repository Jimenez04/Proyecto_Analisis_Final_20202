using Microsoft.EntityFrameworkCore;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Analisis_Final_20202.DA
{
   public class ContextoBaseDeDatos : DbContext
        {
            public DbSet<Empresa> Empresa { get; set; }

            public DbSet<Persona> Persona { get; set; }

            public DbSet<Empleado> Empleado { get; set; }

            public DbSet<Distrito> Distrito { get; set; }

            public DbSet<Canton> Canton { get; set; } 

          public DbSet<Cliente> Cliente { get; set; }

        public DbSet<DetalleFactura> DetalleFactura { get; set; }

        public DbSet<Factura> Factura { get; set; }

        public DbSet<Provincia> Provincia { get; set; }

        public DbSet<Telefono> Telefono { get; set; }

        public DbSet<Correo_Electronico> Correo_Electronico { get; set; }


        public DbSet<Sexo> Sexo { get; set; }

        public DbSet<Inventario> Inventario { get; set; }
        public ContextoBaseDeDatos(DbContextOptions<ContextoBaseDeDatos> opciones) : base(opciones)
            {

            }
        }
    }