using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Test.InventarioBLTest
{
    [TestClass]
     public class InventarioTest:BaseDatosFalsa
    {
        [TestMethod]
        public async Task ListarProductos()  //Prueba de listar y verificar que un producto existe
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Inventario.Add(new Inventario()
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
            });
            contexto.Inventario.Add(new Inventario()
            {
                Codigo_Producto = "EE22",
                Nombre = "Baterias",
                Descripcion = "Baterias triple A",
                Cantidad_Disponible = 4,
                PrecionDeCompra = "1400.00",
                PrecionDeVenta = "1600.00",
                Precio_Compra = 1200.00,
                Precio_Venta = 1400.00,
                ID_Categoria = 1,
                ID_Estado = EstadoInventario.Disponible
            });

            await contexto.SaveChangesAsync();

            var producto = new InventarioBL(contexto);

            Assert.AreEqual(2, producto.ListaInventario().Count);
            Assert.IsTrue(producto.ProductoExiste(new Inventario() { Codigo_Producto = "EE22" }));
        }

        [TestMethod]
        public async Task ListarProductosDisponible()  //Prueba de listar productos disponibles y verificar que un producto existe
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Inventario.Add(new Inventario()
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
            });
            contexto.Inventario.Add(new Inventario()
            {
                Codigo_Producto = "EE22",
                Nombre = "Baterias",
                Descripcion = "Baterias triple A",
                Cantidad_Disponible = 4,
                PrecionDeCompra = "1400.00",
                PrecionDeVenta = "1600.00",
                Precio_Compra = 1200.00,
                Precio_Venta = 1400.00,
                ID_Categoria = 1,
                ID_Estado = EstadoInventario.Sin_existencias
            });

            await contexto.SaveChangesAsync();

            var producto = new InventarioBL(contexto);

            Assert.AreEqual(1, producto.ObtenerProductosDisponibles().Count);
            Assert.AreEqual("MA023", producto.ObtenerProductosDisponibles().First().Codigo_Producto);
        }

        [TestMethod]
        public async Task ListarProductosNodisponibles()  //Prueba de listar productos no disponibles y verificar que un producto existe
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Inventario.Add(new Inventario()
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
            });
            contexto.Inventario.Add(new Inventario()
            {
                Codigo_Producto = "EE22",
                Nombre = "Baterias",
                Descripcion = "Baterias triple A",
                Cantidad_Disponible = 4,
                PrecionDeCompra = "1400.00",
                PrecionDeVenta = "1600.00",
                Precio_Compra = 1200.00,
                Precio_Venta = 1400.00,
                ID_Categoria = 1,
                ID_Estado = EstadoInventario.Sin_existencias
            });

            await contexto.SaveChangesAsync();

            var producto = new InventarioBL(contexto);

            Assert.AreEqual(1, producto.ObtenerProductosSinExistencia().Count);
            Assert.AreEqual("EE22", producto.ObtenerProductosSinExistencia().First().Codigo_Producto);
        }

        [TestMethod]
        public async Task PasarunProductoFueradeServicio()  //Prueba del obtener producto por codigo y el editar
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            Inventario inventario = new  Inventario()
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

            contexto.Inventario.Add(inventario);
            await contexto.SaveChangesAsync();

            var producto = new InventarioBL(contexto);

            Assert.AreEqual("Arroz", producto.ObternerPorCodigo(inventario.Codigo_Producto).Nombre);

            inventario.Nombre = "Tio Pelon";
            contexto.Inventario.Add(inventario);
            Assert.AreEqual("Tio Pelon", producto.ObternerPorCodigo(inventario.Codigo_Producto).Nombre);
        }
    }
}
