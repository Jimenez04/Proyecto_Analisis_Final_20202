using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace Proyecto_Analisis_Final_20202.BL
{
    public class FacturacionBL
    {
        private ContextoBaseDeDatos ElContextoDeBaseDeDatos;
        private EmpresaBL empresa;
        private PersonaBL persona;
        private InventarioBL inventario;

        public FacturacionBL(ContextoBaseDeDatos contexto, EmpresaBL empresa, PersonaBL persona, InventarioBL inventario)
        {
            ElContextoDeBaseDeDatos = contexto;
            this.empresa = empresa;
            this.persona = persona;
            this.inventario = inventario;
        }

        public int Facturar(String Subtotal, int Descuento, String Total, String IdentificacionCliente, List<DetalleFactura> ListaProductos)
        {
            try
            {
                Factura nuevafactura = new Factura();
                Cliente clientedefactura = new Cliente();

                clientedefactura.Consecutivo = nuevafactura.Consecutivo = GenerarConsecutivo();
                nuevafactura.Cedula_Juridica = empresa.ObtenerEmpresa().Cedula_Juridica;
                nuevafactura.ID_Condicion = 1;
                nuevafactura.Plazo_Credito = 0;
                nuevafactura.ID_MetodoPago = 1;
                nuevafactura.SubTotal = Double.Parse(Subtotal.Replace(".", ","));
                nuevafactura.Descuento = Descuento;
                nuevafactura.IVA = 13;
                nuevafactura.Total = Double.Parse(Total.Replace(".", ","));
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


                GenerarXMLDeFactura(nuevafactura);

                return 1;

            }
            catch (Exception)
            {
                return 0;
            }
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



        public List<Factura> ListarFacturas()
        {
            return ElContextoDeBaseDeDatos.Factura.ToList();
        }

        // Metodo de creación del XML 
        public Attachment GenerarXMLDeFactura(Factura factura)
        {
            Cliente cliente = this.persona.ObtenerCliente_porConsecutivo(factura.Consecutivo);
            Persona persona = this.persona.ObtenerPersonaPorCedula(cliente.Cedula);
            Empresa empresa = this.empresa.ObtenerEmpresa();


            List<DetalleFactura> detalleFactura = ElDetalleDeFactura(factura.Consecutivo);

            // Esto es una prueba humilde del XML 

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null); //
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);




            // cuerpo principal
            XmlElement seccionFacturacion = doc.CreateElement(string.Empty, "FacturaElectronica", string.Empty);
            doc.AppendChild(seccionFacturacion);

            //  seccionFacturacion.WriteAttributeString("xmlns", "https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/facturaElectronica");
            //seccionFacturacion.WriteAttributeString("xmlns:ds", "http://www.w3.org/2000/09/xmldsig#");
            /// seccionFacturacion.WriteAttributeString("xmlns:vc", "http://www.w3.org/2007/XMLSchema-versioning");
            //seccionFacturacion.WriteAttributeString("xmlns:xs", "http://www.w3.org/2001/XMLSchema");

            seccionFacturacion.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            seccionFacturacion.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            seccionFacturacion.SetAttribute("xmlns", "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica");



            // Clave 
            XmlElement subseccionclave = doc.CreateElement(string.Empty, "Clave", string.Empty);
            seccionFacturacion.AppendChild(subseccionclave);
            XmlText textClave = doc.CreateTextNode(factura.Clave);
            subseccionclave.AppendChild(textClave);

            // Codigo Actividad 
            XmlElement subseccionactividad = doc.CreateElement(string.Empty, "CodigoActividad", string.Empty);
            seccionFacturacion.AppendChild(subseccionactividad);
            XmlText textactividad = doc.CreateTextNode("722003");
            subseccionactividad.AppendChild(textactividad);

            // Consecutivo
            XmlElement subseccionconsecutivo = doc.CreateElement(string.Empty, "NumeroConsecutivo", string.Empty);
            seccionFacturacion.AppendChild(subseccionconsecutivo);
            XmlText textconsecutivo = doc.CreateTextNode(factura.Consecutivo);
            subseccionconsecutivo.AppendChild(textconsecutivo);

            // Fecha de emision
            XmlElement subseccionrmision = doc.CreateElement(string.Empty, "FechaEmision", string.Empty);
            seccionFacturacion.AppendChild(subseccionrmision);
            XmlText textfecha = doc.CreateTextNode(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            subseccionrmision.AppendChild(textfecha);

            //  Datos de la Empresa
            XmlElement subseccionempresa = doc.CreateElement(string.Empty, "Emisor", string.Empty);
            seccionFacturacion.AppendChild(subseccionempresa);

            // Nombre Emnpresa
            XmlElement nombreempresa = doc.CreateElement(string.Empty, "Nombre", string.Empty);
            XmlText textnombreempresa = doc.CreateTextNode(empresa.Nombre);
            nombreempresa.AppendChild(textnombreempresa);
            subseccionempresa.AppendChild(nombreempresa);

            //Identificacion
            XmlElement identificacionempresa = doc.CreateElement(string.Empty, "Identificacion", string.Empty);
            subseccionempresa.AppendChild(identificacionempresa);

            // Tipo de identificacion 
            XmlElement tipoIdentifiacionempresa = doc.CreateElement(string.Empty, "Tipo", string.Empty);
            XmlText texttipoIdentifiacion = doc.CreateTextNode("02");
            tipoIdentifiacionempresa.AppendChild(texttipoIdentifiacion);
            identificacionempresa.AppendChild(tipoIdentifiacionempresa);

            // Cedula Jurridica 
            XmlElement cedulajuridica = doc.CreateElement(string.Empty, "Numero", string.Empty);
            XmlText textcedulajuridica = doc.CreateTextNode(empresa.Cedula_Juridica);
            cedulajuridica.AppendChild(textcedulajuridica);
            identificacionempresa.AppendChild(cedulajuridica);

            // Nombre Comercial 
            XmlElement nombrecomercial = doc.CreateElement(string.Empty, "NombreComercial", string.Empty);
            XmlText textnombrecomercial = doc.CreateTextNode(empresa.Nombre);
            nombrecomercial.AppendChild(textnombrecomercial);
            subseccionempresa.AppendChild(nombrecomercial);

            // Subseccio Ubicacion dentro de empresa
            XmlElement Ubicacion = doc.CreateElement(string.Empty, "Ubicacion", string.Empty);
            subseccionempresa.AppendChild(Ubicacion);

            XmlElement provinciaempresa = doc.CreateElement(string.Empty, "Provincia", string.Empty);
            XmlText textprovinciaempresa = doc.CreateTextNode(empresa.ID_Provincia.ToString());
            provinciaempresa.AppendChild(textprovinciaempresa);
            Ubicacion.AppendChild(provinciaempresa);

            XmlElement cantonempresa = doc.CreateElement(string.Empty, "Canton", string.Empty);
            XmlText textcantonempresa = doc.CreateTextNode("0" + empresa.ID_Canton.ToString());
            cantonempresa.AppendChild(textcantonempresa);
            Ubicacion.AppendChild(cantonempresa);

            XmlElement distritoempresa = doc.CreateElement(string.Empty, "Distrito", string.Empty);
            XmlText textodistritoempresa = doc.CreateTextNode("0" + empresa.ID_Distrito.ToString());
            distritoempresa.AppendChild(textodistritoempresa);
            Ubicacion.AppendChild(distritoempresa);

            XmlElement barrioempresa = doc.CreateElement(string.Empty, "Barrio", string.Empty);
            XmlText textbarrioempresa = doc.CreateTextNode("01");
            barrioempresa.AppendChild(textbarrioempresa);
            Ubicacion.AppendChild(barrioempresa);

            XmlElement senasempresa = doc.CreateElement(string.Empty, "OtrasSenas", string.Empty);
            XmlText textsenasempresa = doc.CreateTextNode(empresa.Senas_Exactas);
            senasempresa.AppendChild(textsenasempresa);
            Ubicacion.AppendChild(senasempresa);

            XmlElement Telefono = doc.CreateElement(string.Empty, "Telefono", string.Empty);
            subseccionempresa.AppendChild(Telefono);

            XmlElement Paistelefono = doc.CreateElement(string.Empty, "CodigoPais", string.Empty);
            XmlText textpaistelefono = doc.CreateTextNode("506");
            Paistelefono.AppendChild(textpaistelefono);
            Telefono.AppendChild(Paistelefono);

            XmlElement Telefonoempresa = doc.CreateElement(string.Empty, "NumTelefono", string.Empty);
            XmlText textTelefono = doc.CreateTextNode("85660429");
            Telefonoempresa.AppendChild(textTelefono);
            Telefono.AppendChild(Telefonoempresa);

            XmlElement correoempresarial = doc.CreateElement(string.Empty, "CorreoElectronico", string.Empty);
            XmlText textcorreoempresa = doc.CreateTextNode("facturacionjjyf@gmail.com");
            correoempresarial.AppendChild(textcorreoempresa);
            subseccionempresa.AppendChild(correoempresarial);
            // Fin Emisor

            // Persona
            XmlElement subseccionreceptor = doc.CreateElement(string.Empty, "Receptor", string.Empty);
            seccionFacturacion.AppendChild(subseccionreceptor);

            // Nombre Emnpresa
            XmlElement nombrereceptor = doc.CreateElement(string.Empty, "Nombre", string.Empty);
            XmlText textnombrereceptor = doc.CreateTextNode(persona.Nombre1 + " " + persona.Apellido1 + " " + persona.Apellido2);
            nombrereceptor.AppendChild(textnombrereceptor);
            subseccionreceptor.AppendChild(nombrereceptor);

            //Identificacion
            XmlElement subseccionidentificacion = doc.CreateElement(string.Empty, "Identificacion", string.Empty);
            subseccionreceptor.AppendChild(subseccionidentificacion);

            // Tipo de identificacion 
            XmlElement tipoidentificacionpersona = doc.CreateElement(string.Empty, "Tipo", string.Empty);
            XmlText texttipoidpersona = doc.CreateTextNode("01");
            tipoidentificacionpersona.AppendChild(texttipoidpersona);
            subseccionidentificacion.AppendChild(tipoidentificacionpersona);

            // Cedula
            XmlElement cedula = doc.CreateElement(string.Empty, "Numero", string.Empty);
            XmlText textcedula = doc.CreateTextNode(persona.Cedula);
            cedula.AppendChild(textcedula);
            subseccionidentificacion.AppendChild(cedula);


            // Subseccio Ubicacion dentro de empresa
            XmlElement Ubicacionpersona = doc.CreateElement(string.Empty, "Ubicacion", string.Empty);
            subseccionreceptor.AppendChild(Ubicacionpersona);

            XmlElement provinciapersona = doc.CreateElement(string.Empty, "Provincia", string.Empty);
            XmlText textprovinciapersona = doc.CreateTextNode(persona.ID_Provincia.ToString());
            provinciapersona.AppendChild(textprovinciapersona);
            Ubicacionpersona.AppendChild(provinciapersona);

            XmlElement cantonpersona = doc.CreateElement(string.Empty, "Canton", string.Empty);
            XmlText textcantonpersona = doc.CreateTextNode("0" + persona.ID_Canton.ToString());
            cantonpersona.AppendChild(textcantonpersona);
            Ubicacionpersona.AppendChild(cantonpersona);

            XmlElement distritopersona = doc.CreateElement(string.Empty, "Distrito", string.Empty);
            XmlText textdistritopersona = doc.CreateTextNode("0" + persona.ID_Distrito.ToString());
            distritopersona.AppendChild(textdistritopersona);
            Ubicacionpersona.AppendChild(distritopersona);

            XmlElement barriopersona = doc.CreateElement(string.Empty, "Barrio", string.Empty);
            XmlText textbarriopersona = doc.CreateTextNode("01");
            barriopersona.AppendChild(textbarriopersona);
            Ubicacionpersona.AppendChild(barriopersona);

            XmlElement Senaspersona = doc.CreateElement(string.Empty, "OtrasSenas", string.Empty);
            XmlText textsenaspersona = doc.CreateTextNode(persona.SenasExactas);
            Senaspersona.AppendChild(textsenaspersona);
            Ubicacionpersona.AppendChild(Senaspersona);

            XmlElement Telefonopersona = doc.CreateElement(string.Empty, "Telefono", string.Empty);
            subseccionreceptor.AppendChild(Telefonopersona);

            XmlElement Paistelefonopersona = doc.CreateElement(string.Empty, "CodigoPais", string.Empty);
            XmlText textpaistelefonopersona = doc.CreateTextNode("+506");
            Paistelefonopersona.AppendChild(textpaistelefonopersona);
            Telefonopersona.AppendChild(Paistelefonopersona);

            XmlElement Telefonoperosona = doc.CreateElement(string.Empty, "NumTelefono", string.Empty);
            XmlText textTelefonopersona = doc.CreateTextNode(persona.telefono.Numero);
            Telefonoperosona.AppendChild(textTelefonopersona);
            Telefonopersona.AppendChild(Telefonoperosona);

            XmlElement correopersona = doc.CreateElement(string.Empty, "CorreoElectronico", string.Empty);
            XmlText textcorreopersona = doc.CreateTextNode(persona.Correo.Correo);
            correopersona.AppendChild(textcorreopersona);
            subseccionreceptor.AppendChild(correopersona);

            XmlElement condicionventa = doc.CreateElement(string.Empty, "CondicionVenta", string.Empty);
            XmlText textcondicionventa = doc.CreateTextNode("01");
            condicionventa.AppendChild(textcondicionventa);
            seccionFacturacion.AppendChild(condicionventa);

            XmlElement mediopago = doc.CreateElement(string.Empty, "MedioPago", string.Empty);
            XmlText textmediopago = doc.CreateTextNode("01");
            mediopago.AppendChild(textmediopago);
            seccionFacturacion.AppendChild(mediopago);


            // Nueva sección de productos 
            XmlElement Detalleservicio = doc.CreateElement(string.Empty, "DetalleServicio", string.Empty);
            seccionFacturacion.AppendChild(Detalleservicio);

            int numerodelinea = 1;
            foreach (var item in detalleFactura)
            {
                item.SubTotal = item.Cantidad * item.Precio_Unidad;
                Inventario producto = this.inventario.ObternerPorCodigo(item.Codigo_Producto);

                XmlElement LineaDetalle = doc.CreateElement(string.Empty, "LineaDetalle", string.Empty);
                Detalleservicio.AppendChild(LineaDetalle);

                XmlElement numerolinea = doc.CreateElement(string.Empty, "NumeroLinea", string.Empty);
                XmlText textnumerolinea = doc.CreateTextNode(numerodelinea.ToString());
                numerolinea.AppendChild(textnumerolinea);
                LineaDetalle.AppendChild(numerolinea);

                XmlElement codigocomercial = doc.CreateElement(string.Empty, "CodigoComercial", string.Empty);
                LineaDetalle.AppendChild(codigocomercial);

                XmlElement tipocodigocomercial = doc.CreateElement(string.Empty, "Tipo", string.Empty);
                XmlText textcodigocomercial = doc.CreateTextNode("01");
                tipocodigocomercial.AppendChild(textcodigocomercial);
                codigocomercial.AppendChild(tipocodigocomercial);

                XmlElement codigocomercialproducto = doc.CreateElement(string.Empty, "Codigo", string.Empty);
                XmlText textcodigocomercialproducto = doc.CreateTextNode("010");
                codigocomercialproducto.AppendChild(textcodigocomercialproducto);
                codigocomercial.AppendChild(codigocomercialproducto);

                XmlElement cantidad = doc.CreateElement(string.Empty, "Cantidad", string.Empty);
                XmlText textcantidad = doc.CreateTextNode(item.Cantidad.ToString());
                cantidad.AppendChild(textcantidad);
                LineaDetalle.AppendChild(cantidad);

                XmlElement unidadmedida = doc.CreateElement(string.Empty, "UnidadMedida", string.Empty);
                XmlText textunidadmedida = doc.CreateTextNode("Unid");
                unidadmedida.AppendChild(textunidadmedida);
                LineaDetalle.AppendChild(unidadmedida);

                XmlElement descripcion = doc.CreateElement(string.Empty, "Detalle", string.Empty);
                XmlText textdescripcion = doc.CreateTextNode(producto.Descripcion);
                descripcion.AppendChild(textdescripcion);
                LineaDetalle.AppendChild(descripcion);

                XmlElement preciounitario = doc.CreateElement(string.Empty, "PrecioUnitario", string.Empty);
                XmlText textpreciounitario = doc.CreateTextNode(item.Precio_Unidad.ToString().Replace(",", "."));
                preciounitario.AppendChild(textpreciounitario);
                LineaDetalle.AppendChild(preciounitario);

                XmlElement montototal = doc.CreateElement(string.Empty, "MontoTotal", string.Empty);
                XmlText textmontototal = doc.CreateTextNode(item.SubTotal.ToString().Replace(",", "."));
                montototal.AppendChild(textmontototal);
                LineaDetalle.AppendChild(montototal);

                XmlElement subtotal = doc.CreateElement(string.Empty, "SubTotal", string.Empty);
                XmlText textsubtotal = doc.CreateTextNode(item.SubTotal.ToString().Replace(",", "."));
                subtotal.AppendChild(textsubtotal);
                LineaDetalle.AppendChild(subtotal);

                XmlElement impuestodelinea = doc.CreateElement(string.Empty, "Impuesto", string.Empty);
                LineaDetalle.AppendChild(impuestodelinea);

                XmlElement codigoimpuestolinea = doc.CreateElement(string.Empty, "Codigo", string.Empty);
                XmlText textcodigoimpuestolinea = doc.CreateTextNode("01");
                codigoimpuestolinea.AppendChild(textcodigoimpuestolinea);
                impuestodelinea.AppendChild(codigoimpuestolinea);

                XmlElement tarifaimpuesto = doc.CreateElement(string.Empty, "Tarifa", string.Empty);
                XmlText texttarifaimpuesto = doc.CreateTextNode("13.00");
                tarifaimpuesto.AppendChild(texttarifaimpuesto);
                impuestodelinea.AppendChild(tarifaimpuesto);

                double calculo = (item.SubTotal * (0.13));
                XmlElement montoimpuesto = doc.CreateElement(string.Empty, "Monto", string.Empty);
                XmlText textmontoimpuesto = doc.CreateTextNode(Math.Round(calculo, 4).ToString().Replace(",", ".").Trim());
                montoimpuesto.AppendChild(textmontoimpuesto);
                impuestodelinea.AppendChild(montoimpuesto);

                XmlElement totallinea = doc.CreateElement(string.Empty, "MontoTotalLinea", string.Empty);
                XmlText texttotallinea = doc.CreateTextNode(item.Total.ToString().Replace(",", "."));
                totallinea.AppendChild(texttotallinea);

                LineaDetalle.AppendChild(totallinea);

                numerodelinea++;

            }

            XmlElement ResumenFactura = doc.CreateElement(string.Empty, "ResumenFactura", string.Empty);
            seccionFacturacion.AppendChild(ResumenFactura);

            XmlElement totalgrabado = doc.CreateElement(string.Empty, "TotalGravado", string.Empty);
            XmlText texttotalgrabado = doc.CreateTextNode(factura.SubTotal.ToString().Replace(",", "."));
            totalgrabado.AppendChild(texttotalgrabado);
            ResumenFactura.AppendChild(totalgrabado);

            XmlElement TotalExento = doc.CreateElement(string.Empty, "TotalExento", string.Empty);
            XmlText textTotalExento = doc.CreateTextNode("0.00");
            TotalExento.AppendChild(textTotalExento);
            ResumenFactura.AppendChild(TotalExento);

            XmlElement totalventa = doc.CreateElement(string.Empty, "TotalVenta", string.Empty);
            XmlText texttotalventa = doc.CreateTextNode(factura.SubTotal.ToString().Replace(",", "."));
            totalventa.AppendChild(texttotalventa);
            ResumenFactura.AppendChild(totalventa);

            double procentaje = (double) decimal.Divide(factura.Descuento,100);

            XmlElement totaldescuentos = doc.CreateElement(string.Empty, "TotalDescuentos", string.Empty);
            XmlText texttotaldescuentos = doc.CreateTextNode((factura.SubTotal * procentaje).ToString().Replace(",", "."));
            totaldescuentos.AppendChild(texttotaldescuentos);
            ResumenFactura.AppendChild(totaldescuentos);

            XmlElement totalventaneta = doc.CreateElement(string.Empty, "TotalVentaNeta", string.Empty);
            XmlText texttotalventaneta = doc.CreateTextNode(factura.SubTotal.ToString().Replace(",", "."));
            totalventaneta.AppendChild(texttotalventaneta);
            ResumenFactura.AppendChild(totalventaneta);

            XmlElement totalcomprobante = doc.CreateElement(string.Empty, "TotalComprobante", string.Empty);
            XmlText texttotalcomprobante = doc.CreateTextNode(factura.Total.ToString().Replace(",", "."));
            totalcomprobante.AppendChild(texttotalcomprobante);
            ResumenFactura.AppendChild(totalcomprobante);

            // doc.Save("C:/Users/josue/Desktop/" + factura.Consecutivo + ".xml"); // Modificar esta ruta si se va a usar 
            String total = doc.OuterXml;

            //  Conversion a tipo archivo del XML
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlElement));
            ser.Serialize(ms, doc);
            ms.Position = 0;
            var xmlfactura = new System.Net.Mail.Attachment(ms, factura.Consecutivo + ".xml");

            EnviarArchivosDeFactura(xmlfactura,GenerarPDF(factura),persona.Correo.Correo);

            return xmlfactura;
        }

        public void EnviarArchivosDeFactura(Attachment archivoxml, Attachment archivopdf, string correodestinatario)
        {

            string CorreoEmisor = "facturacionjjyf@gmail.com";
            string Contrasena = "facturacion01";


            MailMessage CorreoElectronico = new MailMessage();

            CorreoElectronico.Subject = "Facturación Electronica JJYF ";
            CorreoElectronico.From = new MailAddress(CorreoEmisor);

            CorreoElectronico.Body = "Sus comprobantes elctrónicos son los siguientes";
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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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
            Empresa empresa = this.empresa.ObtenerEmpresa();
            Cliente cliente = this.persona.ObtenerCliente_porConsecutivo(factura.Consecutivo);
            Persona persona = this.persona.ObtenerPersonaPorCedula(cliente.Cedula);

            MemoryStream ms = new MemoryStream();


            var lugar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var expportar = System.IO.Path.Combine(lugar, "Pruebas.pdf");

            PdfWriter pwf = new PdfWriter(ms); // Poner ms para mandar o poner exxportar para hacerlo en menotia

            PdfDocument pdfDocument = new PdfDocument(pwf);
            Document doc = new Document(pdfDocument, PageSize.A4);


            Style estilodefooter = new Style()
            .SetFontSize(10);

            Table tablaprueba = new Table(1).SetBorder(Border.NO_BORDER);

            Cell celdafooter = new Cell()
            .Add(new Paragraph("Emitido por facturación FYJJ " ).SetFontSize(12)).SetBorder(Border.NO_BORDER);
            tablaprueba.AddCell(celdafooter);

             Cell Celda2 = new Cell().Add(new Paragraph("Documento emitido conforme a la resolución de factura electrónica N° DGT-R-033-2019")
             .SetFontSize(8))
             .SetHeight(100).SetBorder(Border.NO_BORDER);
              tablaprueba.AddCell(Celda2);
              pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new TableFooterEventHandler(tablaprueba));


            ImageData imageData = ImageDataFactory.Create(@"Imagen/IconoLayout.png");

            doc.SetMargins(36, 36, 100, 36);
            Image pdfImg = new Image(imageData);

            Style estilodeprimero = new Style()
            .SetMarginLeft(300).SetMarginTop(20);

            Table TablaDatos1 = new Table(1).UseAllAvailableWidth();
            Cell cellaDatos = new Cell().Add(new Paragraph(" Factura Electrónica").SetTextAlignment(TextAlignment.CENTER) 
            .SetFontColor(ColorConstants.WHITE).SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 2))
            .SetBackgroundColor(WebColors.GetRGBColor("#0B73D5")));
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


            Table _table = new Table(14).UseAllAvailableWidth().SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1));

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

            _cell = new Cell(1, 2).Add(new Paragraph("Descuento Linea"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));

            _cell = new Cell(1, 2).Add(new Paragraph("IVA"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));

            _cell = new Cell(1, 2).Add(new Paragraph("Total Linea"));
            _table.AddHeaderCell(_cell.AddStyle(Celdasorganizacionproductos));



            foreach (var item in detalleFactura)
            {
               
                    Inventario producto = this.inventario.ObternerPorCodigo(item.Codigo_Producto);

                    _cell = new Cell(1, 2).Add(new Paragraph(item.Codigo_Producto));
                    _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

                    _cell = new Cell(1, 2).Add(new Paragraph(producto.Nombre));
                    _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));


                    _cell = new Cell(1, 2).Add(new Paragraph(item.Cantidad.ToString()));
                    _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));


                    _cell = new Cell(1, 2).Add(new Paragraph(item.Precio_Unidad.ToString()));
                    _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

                    _cell = new Cell(1, 2).Add(new Paragraph(item.Descuento.ToString() +"%"));
                    _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

                      _cell = new Cell(1, 2).Add(new Paragraph("13%"));
                     _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));

                     _cell = new Cell(1, 2).Add(new Paragraph(item.Total.ToString()));
                     _table.AddCell(_cell.AddStyle(Celdasdatosdeproductos));
            }

            doc.Add(_table);

            // Tabla para los totales. 
            Table TablaMonetaria = new Table(2).SetBorder(new SolidBorder(WebColors.GetRGBColor("#0B73D5"), 1)).SetMarginLeft(335);

            Style celdasmonetarias = new Style()
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(11)
               .SetWidth(188)
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

            celdasmonetairas = new Cell(1,2).Add(new Paragraph(" Total " + " CRC " + factura.Total.ToString()));
            TablaMonetaria.AddHeaderCell(celdasmonetairas).AddStyle(celdasmonetarias); 

            doc.Add(TablaMonetaria); 

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
            table.SetHeight(300).SetWidth(500); 

            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            new Canvas(canvas, new Rectangle(36,60, page.GetPageSize().GetWidth() - 72, 50))
                .Add(table)
                .Close();
        }
    }

}
