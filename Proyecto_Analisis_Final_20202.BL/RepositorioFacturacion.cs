using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Net;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Pdf.Canvas;

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

        public int Facturar(double Subtotal, int Descuento, double Total, String IdentificacionCliente, List<DetalleFactura> ListaProductos)
        {
            Factura nuevafactura = new Factura();
            Cliente clientedefactura = new Cliente();

            clientedefactura.Consecutivo = nuevafactura.Consecutivo = GenerarConsecutivo();
            nuevafactura.Cedula_Juridica = ObtenerEmpresa().Cedula_Juridica;
            nuevafactura.ID_Condicion = 1;
            nuevafactura.Plazo_Credito = 0;
            nuevafactura.ID_MetodoPago = 1;
            nuevafactura.SubTotal = Subtotal;
            nuevafactura.Descuento = Descuento;
            nuevafactura.IVA = 13;
            nuevafactura.Total = Total;
            nuevafactura.Fecha_Emision = DateTime.Now;
            nuevafactura.Clave = GenerarClave(nuevafactura.Consecutivo, nuevafactura.Cedula_Juridica);
            nuevafactura.Codigo_Actividad = 1;

            ElContextoDeBaseDeDatos.Factura.Add(nuevafactura);
            ElContextoDeBaseDeDatos.SaveChanges();
            foreach (var Productos in ListaProductos)
            {
                Productos.Consecutivo = nuevafactura.Consecutivo;
                ElContextoDeBaseDeDatos.DetalleFactura.Add(Productos);
                ElContextoDeBaseDeDatos.SaveChanges();
            }

            clientedefactura.Cedula = IdentificacionCliente;
            clientedefactura.Descuento = 0;
            ElContextoDeBaseDeDatos.Cliente.Add(clientedefactura);
            ElContextoDeBaseDeDatos.SaveChanges();
            // Persona cliente = ObtenerPersonaPorCedula(clientedefactura.Cedula);
            //  EnviarArchivosDeFactura(GenerarXMLDeFactura(nuevafactura), GenerarPDF(nuevafactura),cliente.Correo.Correo);

            GenerarPDF(nuevafactura);


            return 1;
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

            try
            {
                persona.telefono.Cedula = persona.Cedula;
                persona.Correo.Cedula = persona.Cedula;


                ElContextoDeBaseDeDatos.Persona.Add(persona);
                ElContextoDeBaseDeDatos.SaveChanges();

                ElContextoDeBaseDeDatos.Telefono.Add(persona.telefono);
                ElContextoDeBaseDeDatos.SaveChanges();

                ElContextoDeBaseDeDatos.Correo_Electronico.Add(persona.Correo);
                ElContextoDeBaseDeDatos.SaveChanges();
            }
            catch (Exception e)
            {
                //  ElContextoDeBaseDeDatos.RollBack();

                throw;
            }


        }

        public bool PersonaExiste(Persona persona)
        {

            var existencia = ElContextoDeBaseDeDatos.Persona.Find(persona.Cedula);

            if (existencia != null)
            {

                return true;

            }
            else
            {
                return false;

            }
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

        public Persona ObtenerPersonaPorCedula(String Cedula)
        {
            try
            {
                Persona persona;
                persona = ElContextoDeBaseDeDatos.Persona.Find(Cedula);
                persona.Correo = ElContextoDeBaseDeDatos.Correo_Electronico.Find(Cedula);
                persona.telefono = ElContextoDeBaseDeDatos.Telefono.Find(Cedula);
                return persona;
            }
            catch (Exception e)
            {
                return null;
            }

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

        public Inventario ObtenerProductoPorCodigo(string codigo)
        {
            return (from c in ElContextoDeBaseDeDatos.Inventario
                    where c.Codigo_Producto == codigo
                    select c).First();
        }


        public List<Factura> ListarFacturas()
        {
            return ElContextoDeBaseDeDatos.Factura.ToList();
        }

        // Metodo de creación del XML 
        public Attachment GenerarXMLDeFactura(Factura factura)
        {
            Cliente cliente = ObtenerCliente_porConsecutivo(factura.Consecutivo);
            Persona persona = ObtenerPersonaPorCedula(cliente.Cedula);
            Empresa empresa = ObtenerEmpresa();


            List<DetalleFactura> detalleFactura = ElDetalleDeFactura(factura.Consecutivo);

            // Esto es una prueba humilde del XML 

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null); //
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            // cuerpo principal
            XmlElement seccionFacturacion = doc.CreateElement(string.Empty, "FacturaElectronica", string.Empty);
            doc.AppendChild(seccionFacturacion);

            // Consecutivo
            XmlElement subseccionconsecutivo = doc.CreateElement(string.Empty, "Consecutivo", string.Empty);
            seccionFacturacion.AppendChild(subseccionconsecutivo);

            XmlElement consecutivo = doc.CreateElement(string.Empty, "Consecutivo", string.Empty);
            XmlText textconsecutivo = doc.CreateTextNode(factura.Consecutivo);
            consecutivo.AppendChild(textconsecutivo);
            subseccionconsecutivo.AppendChild(consecutivo);

            // Fecha de emision
            XmlElement subseccionrmision = doc.CreateElement(string.Empty, "FechaEmision", string.Empty);
            seccionFacturacion.AppendChild(subseccionrmision);

            XmlElement fechaemision = doc.CreateElement(string.Empty, "Fecha", string.Empty);
            XmlText textfecha = doc.CreateTextNode(factura.Fecha_Emision.ToString());
            fechaemision.AppendChild(textfecha);
            subseccionrmision.AppendChild(fechaemision);

            // Clave 
            XmlElement subseccionclave = doc.CreateElement(string.Empty, "Clave", string.Empty);
            seccionFacturacion.AppendChild(subseccionclave);

            XmlElement clave = doc.CreateElement(string.Empty, "Clave", string.Empty);
            XmlText textClave = doc.CreateTextNode(factura.Clave);
            clave.AppendChild(textClave);
            subseccionclave.AppendChild(clave);

            //  Datos de la Empresa
            XmlElement subseccionempresa = doc.CreateElement(string.Empty, "Emisor", string.Empty);
            seccionFacturacion.AppendChild(subseccionempresa);

            XmlElement cedulajuridicaempresa = doc.CreateElement(string.Empty, "CedulaJuridica", string.Empty);
            XmlText textcedulajuridicaemepresa = doc.CreateTextNode(empresa.Cedula_Juridica);
            cedulajuridicaempresa.AppendChild(textcedulajuridicaemepresa);
            subseccionempresa.AppendChild(cedulajuridicaempresa);

            XmlElement nombreempresa = doc.CreateElement(string.Empty, "NombreEmpresa", string.Empty);
            XmlText textnombreempresa = doc.CreateTextNode(empresa.Nombre);
            nombreempresa.AppendChild(textnombreempresa);
            subseccionempresa.AppendChild(nombreempresa);

            XmlElement codigopais = doc.CreateElement(string.Empty, "País", string.Empty);
            XmlText textcodigopais = doc.CreateTextNode("506");
            codigopais.AppendChild(textcodigopais);
            subseccionempresa.AppendChild(codigopais);

            XmlElement provinciaempresa = doc.CreateElement(string.Empty, "Provincia", string.Empty);
            XmlText textprovinciaempresa = doc.CreateTextNode(empresa.ID_Provincia.ToString());
            provinciaempresa.AppendChild(textprovinciaempresa);
            subseccionempresa.AppendChild(provinciaempresa);

            XmlElement cantonempresa = doc.CreateElement(string.Empty, "Canton", string.Empty);
            XmlText textcantonempresa = doc.CreateTextNode(empresa.ID_Provincia.ToString());
            cantonempresa.AppendChild(textcantonempresa);
            subseccionempresa.AppendChild(cantonempresa);

            XmlElement distritoempresa = doc.CreateElement(string.Empty, "Distrito", string.Empty);
            XmlText textodistritoempresa = doc.CreateTextNode(empresa.ID_Distrito.ToString());
            distritoempresa.AppendChild(textodistritoempresa);
            subseccionempresa.AppendChild(distritoempresa);

            XmlElement senasempresa = doc.CreateElement(string.Empty, "SenasExactas", string.Empty);
            XmlText textsenasempresa = doc.CreateTextNode(empresa.Senas_Exactas);
            senasempresa.AppendChild(textsenasempresa);
            subseccionempresa.AppendChild(senasempresa);

            XmlElement codigopostalempresa = doc.CreateElement(string.Empty, "SenasExactas", string.Empty);
            XmlText textcodigopostalempresa = doc.CreateTextNode(empresa.Codigo_Postal);
            codigopostalempresa.AppendChild(textcodigopostalempresa);
            subseccionempresa.AppendChild(codigopostalempresa);

            XmlElement correoempresarial = doc.CreateElement(string.Empty, "CorreoEmpresa", string.Empty);
            XmlText textcorreoempresa = doc.CreateTextNode("facturacionjjyf@gmail.com");
            correoempresarial.AppendChild(textcorreoempresa);
            subseccionempresa.AppendChild(correoempresarial);

            XmlElement razonsocial = doc.CreateElement(string.Empty, "CorreoEmpresa", string.Empty);
            XmlText textrazonsocial = doc.CreateTextNode(empresa.Razon_Social);
            razonsocial.AppendChild(textrazonsocial);
            subseccionempresa.AppendChild(razonsocial);

            // Persona
            XmlElement subseccionpersona = doc.CreateElement(string.Empty, "Destinatario", string.Empty);
            seccionFacturacion.AppendChild(subseccionpersona);

            XmlElement cedulapersona = doc.CreateElement(string.Empty, "Identificacion", string.Empty);
            XmlText textcedulapersona = doc.CreateTextNode(persona.Cedula);
            cedulapersona.AppendChild(textcedulapersona);
            subseccionpersona.AppendChild(cedulapersona);

            XmlElement numeropersona = doc.CreateElement(string.Empty, "Numero", string.Empty);
            XmlText textnumeropersona = doc.CreateTextNode(persona.telefono.Numero);
            numeropersona.AppendChild(textnumeropersona);
            subseccionpersona.AppendChild(numeropersona);

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

            XmlElement provinciapersona = doc.CreateElement(string.Empty, "Provincia", string.Empty);
            XmlText textprovinciapersona = doc.CreateTextNode(persona.ID_Provincia.ToString());
            provinciapersona.AppendChild(textprovinciapersona);
            subseccionpersona.AppendChild(provinciapersona);

            XmlElement cantonpersona = doc.CreateElement(string.Empty, "Canton", string.Empty);
            XmlText textcantonpersona = doc.CreateTextNode(persona.ID_Canton.ToString());
            cantonpersona.AppendChild(textcantonpersona);
            subseccionpersona.AppendChild(cantonpersona);

            XmlElement distritoperrsona = doc.CreateElement(string.Empty, "Distrito", string.Empty);
            XmlText textdistritopersona = doc.CreateTextNode(persona.ID_Distrito.ToString());
            distritoperrsona.AppendChild(textdistritopersona);
            subseccionpersona.AppendChild(distritoperrsona);

            XmlElement codigopostalpersona = doc.CreateElement(string.Empty, "CodigoPostal", string.Empty);
            XmlText textcodigopostalpersona = doc.CreateTextNode(persona.Codigo_Postal.ToString());
            codigopostalpersona.AppendChild(textcodigopostalpersona);
            subseccionpersona.AppendChild(codigopostalpersona);

            XmlElement correopersona = doc.CreateElement(string.Empty, "Destinatario", string.Empty);
            XmlText textcorreopersona = doc.CreateTextNode(persona.Correo.Correo);
            correopersona.AppendChild(textcorreopersona);
            subseccionpersona.AppendChild(correopersona);


            // Nueva sección de productos 
            XmlElement productos = doc.CreateElement(string.Empty, "Productos", string.Empty);
            seccionFacturacion.AppendChild(productos);

            foreach (var item in detalleFactura)
            {

                Inventario producto = ObtenerProductoPorCodigo(item.Codigo_Producto);

                XmlElement subseccionproductos = doc.CreateElement(string.Empty, "Producto", string.Empty);
                productos.AppendChild(subseccionproductos);

                XmlElement codigoproducto = doc.CreateElement(string.Empty, "CodigoProducto", string.Empty);
                XmlText textcodigoproducto = doc.CreateTextNode(item.Codigo_Producto);
                codigoproducto.AppendChild(textcodigoproducto);
                productos.AppendChild(codigoproducto);

                XmlElement nombreproducto = doc.CreateElement(string.Empty, "NombreProducto", string.Empty);
                XmlText textonombre = doc.CreateTextNode(producto.Nombre);
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


                XmlElement totalproductos = doc.CreateElement(string.Empty, "TotalProductos", string.Empty);
                XmlText texttotalproductos = doc.CreateTextNode(item.Total.ToString());
                totalproductos.AppendChild(texttotalproductos);
                productos.AppendChild(totalproductos);

            }
            // Medio de pago
            XmlElement subseccionmediodepago = doc.CreateElement(string.Empty, "MedioPago", string.Empty);
            seccionFacturacion.AppendChild(subseccionmediodepago);

            XmlElement mediodepagof = doc.CreateElement(string.Empty, "Metodo", string.Empty);
            XmlText textmediodepago = doc.CreateTextNode(factura.ID_MetodoPago.ToString());
            mediodepagof.AppendChild(textmediodepago);
            subseccionmediodepago.AppendChild(mediodepagof);


            //Unidad de medida 
            XmlElement subseccionunidad = doc.CreateElement(string.Empty, "UnidadMedida", string.Empty);
            seccionFacturacion.AppendChild(subseccionunidad);

            XmlElement unidaddemedida = doc.CreateElement(string.Empty, "Unidadmedida", string.Empty);
            XmlText textunidaddemedida = doc.CreateTextNode("Venta de servicio"); //VentaServicios
            unidaddemedida.AppendChild(textunidaddemedida);
            subseccionunidad.AppendChild(unidaddemedida);

            //Plazo 
            XmlElement subseccionplazo = doc.CreateElement(string.Empty, "Plazo", string.Empty);
            seccionFacturacion.AppendChild(subseccionplazo);

            XmlElement plazo = doc.CreateElement(string.Empty, "Plazo", string.Empty);
            XmlText textplazo = doc.CreateTextNode("00");
            plazo.AppendChild(textplazo);
            subseccionplazo.AppendChild(plazo);

            // Condicion de Venta
            XmlElement subseccioncondicion = doc.CreateElement(string.Empty, "CondicionVenta", string.Empty);
            seccionFacturacion.AppendChild(subseccioncondicion);

            XmlElement condicion = doc.CreateElement(string.Empty, "Condicion", string.Empty);
            XmlText textcondicion = doc.CreateTextNode("02");
            condicion.AppendChild(textcondicion);
            subseccioncondicion.AppendChild(condicion);

            //SubTotal
            XmlElement subseccionsubtotal = doc.CreateElement(string.Empty, "SubTotal", string.Empty);
            seccionFacturacion.AppendChild(subseccionsubtotal);

            XmlElement Subtotal = doc.CreateElement(string.Empty, "Subtotal", string.Empty);
            XmlText textSubtotal = doc.CreateTextNode(factura.SubTotal.ToString()); //VentaServicios
            Subtotal.AppendChild(textSubtotal);
            subseccionsubtotal.AppendChild(Subtotal);

            //Moneda 
            XmlElement subseccionmoneda = doc.CreateElement(string.Empty, "Moneda", string.Empty);
            seccionFacturacion.AppendChild(subseccionmoneda);

            XmlElement moneda = doc.CreateElement(string.Empty, "Moneda", string.Empty);
            XmlText textmoneda = doc.CreateTextNode("CR Colónes"); //VentaServicios
            moneda.AppendChild(textmoneda);
            subseccionmoneda.AppendChild(moneda);

            // Total
            XmlElement subsecciontotal = doc.CreateElement(string.Empty, "Total", string.Empty);
            seccionFacturacion.AppendChild(subsecciontotal);

            XmlElement stotal = doc.CreateElement(string.Empty, "Total", string.Empty);
            XmlText texttotal = doc.CreateTextNode(factura.Total.ToString()); //VentaServicios
            stotal.AppendChild(texttotal);
            subsecciontotal.AppendChild(stotal);

            // Impuesto
            XmlElement subseccionimpuesto = doc.CreateElement(string.Empty, "Impuesto", string.Empty);
            seccionFacturacion.AppendChild(subseccionimpuesto);

            XmlElement impuesto = doc.CreateElement(string.Empty, "Impuesto", string.Empty);
            XmlText textimpuesto = doc.CreateTextNode(factura.IVA.ToString()); //VentaServicios
            impuesto.AppendChild(textimpuesto);
            subseccionimpuesto.AppendChild(impuesto);
            //
            XmlElement subsecciondetalle = doc.CreateElement(string.Empty, "Detalle", string.Empty);
            seccionFacturacion.AppendChild(subsecciondetalle);

            XmlElement detalle = doc.CreateElement(string.Empty, "Detalle", string.Empty);
            XmlText textdetalle = doc.CreateTextNode("Otros detalles"); //VentaServicios
            detalle.AppendChild(textdetalle);
            subsecciondetalle.AppendChild(detalle);

            doc.Save("C:/Users/josue/Desktop/" + factura.Consecutivo + ".xml"); // Modificar esta ruta si se va a usar 
            String total = doc.OuterXml;

            //  Conversion a tipo archivo del XML
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlElement));
            ser.Serialize(ms, doc);
            ms.Position = 0;
            var xmlfactura = new System.Net.Mail.Attachment(ms, factura.Consecutivo + ".xml");

            return xmlfactura;
        }

        public void EnviarArchivosDeFactura(Attachment archivoxml, Attachment archivopdf, string correodestinatario)
        {

            string CorreoEmisor = "facturacionjjyf@gmail.com";
            string Contrasena = "facturacion01";


            MailMessage CorreoElectronico = new MailMessage();

            CorreoElectronico.Subject = "Prueba de envio XML";
            CorreoElectronico.From = new MailAddress(CorreoEmisor);

            CorreoElectronico.Body = "XML es el siguiente:";
            CorreoElectronico.To.Add(new MailAddress(correodestinatario));

            CorreoElectronico.Attachments.Add(archivoxml);
            CorreoElectronico.Attachments.Add(archivopdf);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(CorreoEmisor, Contrasena);
            smtp.Credentials = nc;
            smtp.Send(CorreoElectronico);

        }

        public string GenerarConsecutivo()
        {
            try
            {
                long actuales = ListarFacturas().Count + 1;
                String ConsecutivoDeFactura;

                if (actuales < 9999999999)
                {
                    ConsecutivoDeFactura = LlenadoDeRestantesDeFactura(actuales.ToString());
                }
                else
                {
                    String nuevosciclos = "1";
                    ConsecutivoDeFactura = LlenadoDeRestantesDeFactura(nuevosciclos.ToString());
                }
                return (CasaMatriz() + Terminales() + TipoDeDocumento() + ConsecutivoDeFactura);
            }
            catch (Exception e)
            {
                return null;
            }

        }
        private string LlenadoDeRestantesDeFactura(String siguientefactura)
        {
            int digitosfaltantes = (10 - siguientefactura.Length);
            string numerofactura = siguientefactura;

            for (int i = 0; i < digitosfaltantes; i++)
            {
                numerofactura = String.Concat(0.ToString(), numerofactura);
            }

            return numerofactura;
        }

        private String CasaMatriz()
        {
            return "001";
        }

        private String Terminales()
        {
            return "00001";
        }

        private String TipoDeDocumento()
        {
            return "01";
        }

        public String GenerarClave(String consecutivo, String cedulaJuridica)
        {
            try
            {
                return ("506" + FechaActual() + "00" + cedulaJuridica + consecutivo + CodigoSituacion() + GenerarCodigoSeguridad());
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private String CodigoSituacion()
        {
            return "1";
        }

        private String GenerarCodigoSeguridad()
        {
            Random GeneradorRandom = new Random();
            String caracteres = "1234567890";
            int longitud = caracteres.Length;
            int largocodigo = 8;
            char caracter;
            String CodigoSeguridad = "";
            for (int i = 0; i < largocodigo; i++)
            {
                caracter = caracteres[GeneradorRandom.Next(longitud)];
                CodigoSeguridad += caracter.ToString();
            }
            return CodigoSeguridad;
        }
        private String FechaActual()
        {
            return DateTime.Now.ToString("ddMMyy");
        }

        private Attachment GenerarPDF(Factura factura)
        {
            List<DetalleFactura> detalleFactura = ElDetalleDeFactura(factura.Consecutivo);
            Empresa empresa = ObtenerEmpresa();
            Cliente cliente = ObtenerCliente_porConsecutivo(factura.Consecutivo);
            Persona persona = ObtenerPersonaPorCedula(cliente.Cedula);

            MemoryStream ms = new MemoryStream();


            var lugar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var expportar = System.IO.Path.Combine(lugar, "Pruebas.pdf");

            PdfWriter pwf = new PdfWriter(expportar); // Poner ms para mandar o poner exxportar para hacerlo en menotia

            PdfDocument pdfDocument = new PdfDocument(pwf);
            Document doc = new Document(pdfDocument, PageSize.LETTER);
            doc.SetMargins(10, 35, 70, 35);

            ImageData imageData = ImageDataFactory.Create(@"Imagen/IconoLayout.png");


            Image pdfImg = new Image(imageData);

            Style estilodeprimero = new Style()
            .SetMarginLeft(300).SetMarginTop(20);


            Table TablaDatos1 = new Table(1).UseAllAvailableWidth();
            Cell cellaDatos = new Cell().Add(new Paragraph(" Factura Electrónica").SetTextAlignment(TextAlignment.CENTER).SetFontColor(ColorConstants.WHITE).SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2)).SetBackgroundColor(WebColors.GetRGBColor("#0B73D5")));
            TablaDatos1.AddCell(cellaDatos);


            

            doc.Add(TablaDatos1);

            Table table1 = new Table(6).UseAllAvailableWidth().SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2));

            Cell celdaImagen = new Cell().Add(pdfImg.SetHeight(120).SetWidth(120)).SetBorder(Border.NO_BORDER);
            table1.AddCell(celdaImagen);

            Cell celdaempresa = new Cell()
               .Add(new Paragraph("Cedula Jurídica " + empresa.Cedula_Juridica).SetFontSize(11).SetMarginLeft(220).SetMarginTop(10))
               .Add(new Paragraph("\n Numero:  " + " (506) 85660429 ").SetFontSize(11).SetMarginLeft(220))
               .Add(new Paragraph(" Coreo: " + " facturacionjjyf@gmail.com").SetFontSize(11).SetMarginLeft(220))
               .Add(new Paragraph("\n  Dirección " + empresa.Senas_Exactas).SetFontSize(11).SetMarginLeft(220))
               .SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2))
               .SetBorderRight(Border.NO_BORDER)
               .SetBorder(Border.NO_BORDER)
               ;

            table1.AddCell(celdaempresa);
            doc.Add(table1);


            //.SetMarginLeft(293)

            /**
            Cell cell1 = new Cell().Add(new Paragraph(empresa.Nombre +
                "\nCedula Jurídica: " + empresa.Cedula_Juridica +
                "\n Número: " + "(506) 85442065" +
                "\n Correo: " + "facturacionjjyf@gmail.com" +
                "\n Dirección: " + empresa.Senas_Exactas).SetFontSize(10).SetFontColor(ColorConstants.BLACK)).SetFontSize(15)
                .SetBorder(Border.NO_BORDER);
                table1.AddCell(cell1);
                 doc.Add(table1);

            **/



            Table TablaDatos = new Table(6).UseAllAvailableWidth().SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1));


            Cell nuevacell = new Cell()
               .Add(new Paragraph("Consecutivo " + factura.Consecutivo))
               .Add(new Paragraph("Clave: " + factura.Clave).SetFontSize(9))
               .Add(new Paragraph("\n" + " Receptor"))
               .Add(new Paragraph(persona.Nombre1 + " " + persona.Apellido2).SetFontSize(10))
               .Add(new Paragraph(" Cedula: " + persona.Cedula).SetFontSize(10))
               .Add(new Paragraph(" Correo: " + persona.Correo.Correo).SetFontSize(10))
               .SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2))
               .SetBorderRight(Border.NO_BORDER)
                ;
            TablaDatos.AddCell(nuevacell);

            Cell nuevacellda = new Cell()
               .Add(new Paragraph("Fecha " + DateTime.Now.ToString("dd/MM/yyyy")).SetFontSize(10))
               .Add(new Paragraph("\n Medio de pago " + "Efectivo").SetFontSize(10))
               .Add(new Paragraph("Condición venta " + "Contado").SetFontSize(10))
               .Add(new Paragraph("\n Teléfono" + "+(506)" + persona.telefono.Numero).SetFontSize(10))
               .Add(new Paragraph(" Cedula: " + persona.Cedula).SetFontSize(10))
               .Add(new Paragraph("Dirección " + persona.SenasExactas).SetFontSize(10))
               .SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2))
               .SetBorderLeft(Border.NO_BORDER)
               ;
            TablaDatos.AddCell(nuevacellda);

            doc.Add(TablaDatos);


            // Tabla de Productos
            doc.Add(new Paragraph());


            Table _table = new Table(8).UseAllAvailableWidth().SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1));

            Style Celdasorganizacionproductos = new Style()
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(11)
              .SetBorder(Border.NO_BORDER)
              .SetBackgroundColor(WebColors.GetRGBColor("#0B73D5"))
              .SetFontColor(ColorConstants.WHITE)
              ;

            Style Celdasdatosdeproductos = new Style()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(10);

            Cell _cell = new Cell(1, 2).Add(new Paragraph("Codigo"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));

            _cell = new Cell(1, 2).Add(new Paragraph("Nombre"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));

            _cell = new Cell(1, 2).Add(new Paragraph("Cantidad"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));

            _cell = new Cell(1, 2).Add(new Paragraph("Precio"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));


            foreach (var item in detalleFactura)
            {

                Inventario producto = ObtenerProductoPorCodigo(item.Codigo_Producto);

                _cell = new Cell(1, 2).Add(new Paragraph(item.Codigo_Producto));
                _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

                _cell = new Cell(1, 2).Add(new Paragraph(producto.Nombre));
                _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));


                _cell = new Cell(1, 2).Add(new Paragraph(item.Cantidad.ToString()));
                _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));


                _cell = new Cell(1, 2).Add(new Paragraph(item.Precio_Unidad.ToString()));
                _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

            }

            doc.Add(_table);

            // Tabla para los totales. 
            Table TablaMonetaria = new Table(2).SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1)).SetMarginLeft(335);

            Style celdasmonetarias = new Style()
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(11)
               .SetWidth(100)
               .SetBackgroundColor(WebColors.GetRGBColor("#0B73D5"))
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(ColorConstants.WHITE);


            Cell celdasmonetairas = new Cell().Add(new Paragraph("SubTotal"));
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));


            celdasmonetairas = new Cell().Add(new Paragraph(" CRC " + factura.SubTotal.ToString())).SetFontSize(9);
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));

            celdasmonetairas = new Cell().Add(new Paragraph("Descuento"));
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));


            celdasmonetairas = new Cell().Add(new Paragraph(factura.Descuento.ToString() + "%")).SetFontSize(10);
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));


            celdasmonetairas = new Cell().Add(new Paragraph("IVA"));
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));

            celdasmonetairas = new Cell().Add(new Paragraph(factura.IVA.ToString() + "%")).SetFontSize(10);
            TablaMonetaria.AddHeaderCell(celdasmonetairas.AddStyle(celdasmonetarias));

            doc.Add(TablaMonetaria);

            // Tabla para ubicación del total 
            Table TablaTotal = new Table(1).SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1)).SetMarginLeft(335);

            Style celdatotal = new Style()
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(11)
               .SetWidth(202)
               .SetBackgroundColor(WebColors.GetRGBColor("#0B73D5"))
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(ColorConstants.WHITE);


            Cell celdaTotal = new Cell().Add(new Paragraph(" Total " + " CRC " + factura.Total.ToString()));
            TablaTotal.AddHeaderCell(celdaTotal.AddStyle(celdatotal));
            doc.Add(TablaTotal);

            Table tablaprueba = new Table(2);

            Cell cell = new Cell().Add(new Paragraph("This is a test doc"));
            cell.SetBackgroundColor(ColorConstants.ORANGE);
            tablaprueba.AddCell(cell);

            cell = new Cell().Add(new Paragraph("This is a copyright notice"));
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            tablaprueba.AddCell(cell);

            pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new TableFooterEventHandler(tablaprueba));




            doc.Close();
            byte[] byteStream = ms.ToArray();
            ms = new MemoryStream();
            ms.Write(byteStream, 0, byteStream.Length);
            ms.Position = 0;

            var pdffactura = new System.Net.Mail.Attachment(ms, factura.Consecutivo + ".pdf");

            return pdffactura;


        }


    }

    public class TableFooterEventHandler : IEventHandler
    {
        private Table table;

        public TableFooterEventHandler(Table table)
        {
            this.table = table;
        }

        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            new Canvas(canvas, new Rectangle(36, 20, page.GetPageSize().GetWidth() - 72, 50))
                .Add(table)
                .Close();
        }
    }

}



