using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;

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
            producto.PrecionDeCompra = producto.Precio_Compra.ToString();
            producto.PrecionDeVenta = producto.Precio_Venta.ToString();
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
            } else if (producto.Cantidad_Disponible == 0 && producto.ID_Estado == EstadoInventario.Disponible)
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

        public bool ProductoExiste(Inventario producto)
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

            GenerarXMLDeFactura(BuscarFactura("1000000000000000000000000000000000000000000000001"));
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
        public Cliente ObtenerCliente_porConsecutivo(string consecutivo)
        {
            return (from c in ElContextoDeBaseDeDatos.Cliente
                   where c.Consecutivo == consecutivo 
                   select c).First();
        }

        public List<DetalleFactura> ElDetalleDeFactura(string consecutivo)
        {
            return (from c in ElContextoDeBaseDeDatos.DetalleFactura
                    where c.Consecutivo == consecutivo
                    select c).ToList();
        }

        public Factura BuscarFactura(string consecutivo)
        {
            return (from c in ElContextoDeBaseDeDatos.Factura
                    where c.Consecutivo == consecutivo
                    select c).First();
        }

        // Metodo de creación del XML 
        public String GenerarXMLDeFactura(Factura factura)
        {
            Cliente cliente = ObtenerCliente_porConsecutivo(factura.Consecutivo);

            Persona persona = ObtenerPersonaPorCedula(cliente.Cedula);

            Empresa empresa = ObtenerEmpresa();

            List <DetalleFactura> detalleFactura = ElDetalleDeFactura(factura.Consecutivo); 

            



            // Esto es una prueba humilde del XML 
            List<DetalleFactura> DT = new List< DetalleFactura>();
            



            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement seccionpersona = doc.CreateElement(string.Empty, "Facturacion", string.Empty);
            doc.AppendChild(seccionpersona);

            // Fecha 
            // Datos de la empresa
            //Consecutivo 
            // MedioPago 
            // Unidad de medida
            //Subtotal
            //MontoTotal
            //Impuesto
            //Detalle

            XmlElement subseccionpersona = doc.CreateElement(string.Empty, "Persona", string.Empty);
            seccionpersona.AppendChild(subseccionpersona);


            XmlElement nombrepersona = doc.CreateElement(string.Empty, "Nombre", string.Empty);
            XmlText textnombrepersona = doc.CreateTextNode(persona.Nombre1);
            nombrepersona.AppendChild(textnombrepersona);
            subseccionpersona.AppendChild(nombrepersona);

            XmlElement Apellido1 = doc.CreateElement(string.Empty, "Apellido1", string.Empty);
            XmlText textapellido1 = doc.CreateTextNode(persona.Apellido1);
            Apellido1.AppendChild(textapellido1);
            subseccionpersona.AppendChild(Apellido1);

            XmlElement Apellido2 = doc.CreateElement(string.Empty, "Apellido2", string.Empty);
            XmlText textapellido2 = doc.CreateTextNode(persona.Apellido2);
            Apellido2.AppendChild(textapellido2);
            subseccionpersona.AppendChild(Apellido2);
             
           String correodestino = (from c in ElContextoDeBaseDeDatos.Correo_Electronico
                          where c.Cedula == persona.Cedula
                          select c.Correo).ToString();

            XmlElement correopersona = doc.CreateElement(string.Empty, "Destinatario", string.Empty);
            XmlText textcorreopersona = doc.CreateTextNode(persona.Correo.Correo);
            correopersona.AppendChild(textcorreopersona);
            subseccionpersona.AppendChild(correopersona);


            // Nueva sección de productos 
            XmlElement productos = doc.CreateElement(string.Empty, "Productos", string.Empty);
            seccionpersona.AppendChild(productos);

            XmlElement subseccionproductos = doc.CreateElement(string.Empty, "Producto", string.Empty);
            productos.AppendChild(subseccionproductos);
            /**
             foreach (var item in DT)
             {
                 XmlElement subseccionproductos = doc.CreateElement(string.Empty, "Producto", string.Empty);
                 productos.AppendChild(subseccionproductos);

                 XmlElement codigoproducto = doc.CreateElement(string.Empty, "CodigoProducto", string.Empty);
                 XmlText textcodigoproducto = doc.CreateTextNode(item.Codigo_Producto);
                 codigoproducto.AppendChild(textcodigoproducto);
                 productos.AppendChild(codigoproducto);


                 XmlElement nombreproducto = doc.CreateElement(string.Empty, "NombreProducto", string.Empty);
                 XmlText textonombre = doc.CreateTextNode(item.Codigo_Producto);
                 nombreproducto.AppendChild(textonombre);
                 productos.AppendChild(nombreproducto);

                 XmlElement cantidad = doc.CreateElement(string.Empty, "Cantidad", string.Empty);
                 XmlText textocantidad = doc.CreateTextNode(item.Cantidad.ToString());
                 cantidad.AppendChild(textocantidad);
                 productos.AppendChild(cantidad);

                 XmlElement precio = doc.CreateElement(string.Empty, "Precio", string.Empty);
                 XmlText textoprecio = doc.CreateTextNode(item.Precio_Unidad.ToString());
                 precio.AppendChild(textoprecio);
                 productos.AppendChild(precio); 

             }
            **/

            doc.Save("C:/Users/josue/Desktop/FC/" + persona.Cedula+".xml"); // Modificar esta ruta si se va a usar 
              String total = doc.OuterXml;

            return total;


        }

        
    }
}
