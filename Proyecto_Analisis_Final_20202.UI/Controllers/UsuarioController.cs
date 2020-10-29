using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    [Authorize(Roles = "Administrador, Empleado")]
    public class UsuarioController : Controller
    {
        private readonly FacturacionBL RepositorioFacturacion;
        private readonly PersonaBL RepositorioPersona;
        private readonly InventarioBL RepositorioInventario;

        public UsuarioController(FacturacionBL Facturacion, PersonaBL Persona, InventarioBL Inventario)
        {
                this.RepositorioFacturacion = Facturacion;
                this.RepositorioPersona = Persona;
                this.RepositorioInventario = Inventario;
        }
        public IActionResult VentanaPrincipal()
        {
            List<DetalleFactura> ListadetalleFacturas = new List<DetalleFactura>();
            return View(ListadetalleFacturas);
        }

       

        [HttpPost]
        public ActionResult Facturaracion(string SubTotal, int Descuento, string Total,  string IdentificacionCliente, List<DetalleFactura> ListaProductos)
        {
            Persona persona = new Persona();
            persona.Cedula = IdentificacionCliente.ToString().Trim();
            try
            {
                if (RepositorioPersona.PersonaExiste(persona))
                {
                    RepositorioFacturacion.Facturar( SubTotal, Descuento, Total, IdentificacionCliente.Trim(), ListaProductos);
                    return Json("La compra se ha efectuado con exito");
                }
                else
                {
                    return Json("La persona no existe en sistema");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json("Error inesperado, verifique los datos");
            }
        }
    }
}
