using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto_Analisis_Final_20202.BL
{
    public class RepositorioFacturacion : IRepositorioFacturacion
    {
        private ContextoBaseDeDatos ElContextoDeBaseDeDatos;
        public RepositorioFacturacion(ContextoBaseDeDatos contexto)
        {
            ElContextoDeBaseDeDatos = contexto;
        }

        public List<Canton> ListadoDeCantones(int ID_Provincia)
        {
            return (from c in ElContextoDeBaseDeDatos.Canton
                    where (c.ID_Provincia == ID_Provincia)
                    select c).ToList(); 
        }

        public List<Distrito> ListadoDeDistritos(int ID_Provincia, int ID_Canton)
        {
            return (from c in ElContextoDeBaseDeDatos.Distrito
                    where (c.ID_Canton == ID_Canton)
                    && (c.ID_Provincia == ID_Provincia)
                    select c).ToList();
        }

        public List<Provincia> ListadoDeProvincias()
        {
            return ElContextoDeBaseDeDatos.Provincia.ToList();
        }

        public List<Sexo> ListadoDeSexos()
        {
            return ElContextoDeBaseDeDatos.Sexo.ToList();
        }
        public bool VerificaciondeExistenciaEmpresa(string Cedula_Judica)
        {
          Empresa empresa = new Empresa();
            empresa = ElContextoDeBaseDeDatos.Empresa.Find(Cedula_Judica);
           
            if (empresa != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CreacionDeCuentaEmpresarial(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial)
        {
            Empresa NuevaEmpresa = new Empresa();
            NuevaEmpresa.Cedula_Juridica = nuevaCuentaEmpresarial.Cedula;
            //NuevaEmpresa.Nombre = nuevaCuentaEmpresarial.NombreOrganizacion;
           // EnvioDeDatosParaLoginCorreo(usuario_Empresa.Clave, usuario_Empresa.Nombre_Usuario, nuevaCuentaEmpresarial.CorreoElectronico);
            //InicioSecion(usuario_Empresa.Clave, usuario_Empresa.Nombre_Usuario);

            //-------  Datos de Invento Ficticios----------------- 

            NuevaEmpresa.Razon_Social = "Ingrese su razón";
            NuevaEmpresa.ID_Provincia = 1;
            NuevaEmpresa.ID_Canton = 1;
            NuevaEmpresa.ID_Distrito = 1;
            NuevaEmpresa.Senas_Exactas = "Ingrese sus señas";
            NuevaEmpresa.Codigo_Postal = "10101";

            //------- Termina la mamadera ----------------- 

            ElContextoDeBaseDeDatos.Empresa.Add(NuevaEmpresa);
            ElContextoDeBaseDeDatos.SaveChanges();

            /* if (false) 
             {

                 return true;
             }
             else
             {
                 return false;
             }
            */
            return true;
        }

        public string GeneradorDeContrasena()
        {
            Random GeneradorRandom = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int longitud = caracteres.Length;
            char caracter;
            int LargoContrasena = 3;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < LargoContrasena; i++)
            {
                caracter = caracteres[GeneradorRandom.Next(longitud)];
                contraseniaAleatoria += caracter.ToString();
            }
            caracteres = "1234567890";
            longitud = caracteres.Length;
            LargoContrasena = 4;
            for (int i = 0; i < LargoContrasena; i++)
            {
                caracter = caracteres[GeneradorRandom.Next(longitud)];
                contraseniaAleatoria += caracter.ToString();
            }
            return contraseniaAleatoria;
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

        public List<DetalleFactura> Carrito(String codigo)
        {
            Inventario producto;
            producto = ElContextoDeBaseDeDatos.Inventario.Find(codigo);
            DetalleFactura DTT = new DetalleFactura();
            DTT.Codigo_Producto = producto.Codigo_Prodcuto;
            DTT.Nombre_Articulo = producto.Nombre;
           
            return null;
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

            if (producto.Cantidad_Disponible > 0 )
            {
                producto.ID_Estado = EstadoInventario.Disponible;
            }else if (producto.Cantidad_Disponible == 0 && producto.ID_Estado == EstadoInventario.Disponible)
            {
            producto.ID_Estado = EstadoInventario.Sin_existencias;
            }
            ElContextoDeBaseDeDatos.Inventario.Update(producto);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public List<Persona> ListarPersonas()
        {
            List<Persona> laListaDePersonas;
            laListaDePersonas = ElContextoDeBaseDeDatos.Persona.ToList();
            foreach (var item in laListaDePersonas)
            {
                item.Correo = ElContextoDeBaseDeDatos.Correo_Electronico.Find(item.Cedula);
            }
            return laListaDePersonas;
        }

        public void AgregarPersonas(Persona persona)
        {
            persona.telefono.Cedula = persona.Cedula;
            persona.Correo.Cedula = persona.Cedula;

            ElContextoDeBaseDeDatos.Persona.Add(persona);
            ElContextoDeBaseDeDatos.SaveChanges();
            ElContextoDeBaseDeDatos.Telefono.Add(persona.telefono);
            ElContextoDeBaseDeDatos.Correo_Electronico.Add(persona.Correo);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public bool PersonaExiste(Persona persona)
        {
            
            var existencia = ElContextoDeBaseDeDatos.Persona.Find(persona.Cedula);

            if (existencia != null) {

                return true;

            } else
            {
                return false;

            }                 
        }

        public bool ProductoExiste( Inventario producto)
        {

            var existencia = ElContextoDeBaseDeDatos.Inventario.Find(producto.Codigo_Prodcuto);

            if (existencia != null)
            {

                return true;

            }
            else
            {
                return false;

            }
        }

        public Persona ObtenerPersonaPorCedula(String Cedula)
        {
            Persona persona;
            persona = ElContextoDeBaseDeDatos.Persona.Find(Cedula);
            persona.Correo = ElContextoDeBaseDeDatos.Correo_Electronico.Find(Cedula);
            persona.telefono = ElContextoDeBaseDeDatos.Telefono.Find(Cedula);
            return persona;
        }

        public void EditarPersona(Persona persona)
        {
            ElContextoDeBaseDeDatos.Persona.Update(persona);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public Empresa ObtenerEmpresa() 
        {
            return ElContextoDeBaseDeDatos.Empresa.Find("1234567890");
        }

        public void EditarEmpresa(Empresa empresa)
        {
            ElContextoDeBaseDeDatos.Empresa.Update(empresa);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        //Para Inventario
        

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
