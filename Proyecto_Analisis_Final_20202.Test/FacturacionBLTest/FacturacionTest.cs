using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Test.FacturacionBLTest
{
    [TestClass]
    public class FacturacionTest:BaseDatosFalsa
    {
        [TestMethod]
        public async Task Facturacion()  //Prueba del repositorio encargado de facturacion, PDF y XML
        {
            var nombre = Guid.NewGuid().ToString();
            var contextoFactura = CrearContexto(nombre);

            Correo_Electronico nuevocorreo = new Correo_Electronico() { Cedula = "504250352", Correo = "jose.040199@hotmail.com" };
            Telefono nuevotelefono  = new Telefono() { Cedula = "504250352", Numero = "88888888" };
            contextoFactura.Telefono.Add(nuevotelefono);
            contextoFactura.Correo_Electronico.Add(nuevocorreo);

            Persona nuevapersona = new Persona()
            {
                Cedula = "504250352",
                Nombre1 = "Jose",
                Nombre2 = "Enrique",
                Apellido1 = "Jimenez",
                Apellido2 = "Soto",
                ID_Canton = 5,
                Codigo_Postal = "50505",
                ID_Distrito = 5,
                ID_Provincia = 5,
                ID_Sexo = 1,
                SenasExactas = "Por el coco",
            };

            contextoFactura.Persona.Add(nuevapersona);
         //   var repositorioPersonasBL = new PersonaBL(contextoFactura);
           // repositorioPersonasBL.AgregarPersonas(nuevapersona);


            Inventario inventario = new Inventario()
            {
                Codigo_Producto = "MA023",
                Nombre = "Arroz",
                Descripcion = "Arroz Tio Pelon 2kg",
                Cantidad_Disponible = 5,
                PrecionDeCompra = "1000.00",
                PrecionDeVenta = "1200.00",
                Precio_Compra = 1000.00,
                Precio_Venta = 1200.00,
                ID_Categoria = 1,
                ID_Estado = EstadoInventario.Disponible
            };
            var repositorioInventarioBL = new InventarioBL(contextoFactura);
            repositorioInventarioBL.AgregarInventario(inventario);

            Empresa crearempresa = new Empresa()
            {
                Cedula_Juridica = "1234567890",
                Nombre = "Patitos S.A",
                Razon_Social = "Ayuda infantil",
                ID_Provincia = 5,
                ID_Canton = 5,
                ID_Distrito = 5,
                Codigo_Postal = "50505",
                Senas_Exactas = "Por el Coco"
            };

            contextoFactura.Empresa.Add(crearempresa);
            await contextoFactura.SaveChangesAsync();

            var repoEmpresa = new EmpresaBL(contextoFactura);
            var repoPersona = new PersonaBL(contextoFactura);
            var repoinventario = new InventarioBL(contextoFactura);
            var repositorioFacturacionBL = new FacturacionBL(contextoFactura, repoEmpresa, repoPersona,  repoinventario);

            DetalleFactura detalles = new DetalleFactura()
            {
                Codigo_Producto = "MA023",
                Cantidad = 2,
                Descuento = 0,
                Impuesto_Producto =
                13,
                Nombre_Articulo = "Arroz",
                Precio_Unidad = 1200.00,
                SubTotal = 2400.00,
                Total = 2400.00
            };

            List<DetalleFactura> ListaProductos = new List<DetalleFactura>();
            ListaProductos.Add(detalles);
            Assert.AreEqual(0, repositorioFacturacionBL.Facturar("2400.00", 0 , "2400.00" , "504250352" , ListaProductos)); //fallas en el PDF
        }
    }
}
