using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Analisis_Final_20202.BL
{
    public class InventarioBL
    {
        private ContextoBaseDeDatos ElContextoDeBaseDeDatos;
        public InventarioBL(ContextoBaseDeDatos contexto)
        {
            ElContextoDeBaseDeDatos = contexto;
        }

        //Metodos para el inventario
        public List<Inventario> ListaInventario()
        {
            List<Inventario> laListaDeInventario;
            laListaDeInventario = ElContextoDeBaseDeDatos.Inventario.ToList();
            return laListaDeInventario;
        }

        public Inventario ObternerPorCodigo(String codigo)
        {
            Inventario producto;
            producto = ElContextoDeBaseDeDatos.Inventario.Find(codigo);
            if (producto != null)
            {
                producto.PrecionDeCompra = producto.Precio_Compra.ToString();
                producto.PrecionDeVenta = producto.Precio_Venta.ToString();
            }
            return producto;
        }

        public void AgregarInventario(Inventario inventario)
        {
            inventario.Precio_Compra = double.Parse(inventario.PrecionDeCompra);
            inventario.Precio_Venta = double.Parse(inventario.PrecionDeVenta);
            inventario.ID_Estado = EstadoInventario.Disponible;
            inventario.ID_Categoria = 1;

            ElContextoDeBaseDeDatos.Inventario.Add(inventario);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public void EditarProducto(Inventario producto)
        {
            producto.Precio_Compra = double.Parse(producto.PrecionDeCompra);
            producto.Precio_Venta = double.Parse(producto.PrecionDeVenta);

            if (producto.Cantidad_Disponible > 0)
            {
                producto.ID_Estado = EstadoInventario.Disponible;
            }
            else if (producto.Cantidad_Disponible == 0 && producto.ID_Estado == EstadoInventario.Disponible)
            {
                producto.ID_Estado = EstadoInventario.Sin_existencias;
            }
            ElContextoDeBaseDeDatos.Inventario.Update(producto);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public bool ProductoExiste(Inventario producto)
        {
            var existencia = ElContextoDeBaseDeDatos.Inventario.Find(producto.Codigo_Producto);

            if (existencia != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FueraServicio(string Codigo_Prodcuto)
        {
            Inventario ArticuloFueraDeServicio;
            ArticuloFueraDeServicio = ObternerPorCodigo(Codigo_Prodcuto);
            ArticuloFueraDeServicio.ID_Estado = EstadoInventario.Sin_existencias;
            ArticuloFueraDeServicio.Cantidad_Disponible = 0;
            ElContextoDeBaseDeDatos.Inventario.Update(ArticuloFueraDeServicio);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public List<Inventario> ObtenerProductosSinExistencia()
        {
            return (from c in ElContextoDeBaseDeDatos.Inventario
                    where c.ID_Estado == EstadoInventario.Sin_existencias
                    select c).ToList();
        }

        public List<Inventario> ObtenerProductosDisponibles()
        {
            return (from c in ElContextoDeBaseDeDatos.Inventario
                    where c.ID_Estado == EstadoInventario.Disponible
                    select c).ToList();
        }

    }
}
