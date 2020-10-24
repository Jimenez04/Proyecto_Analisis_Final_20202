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
        private readonly IRepositorioFacturacion RepositorioFacturacion;

        public UsuarioController(IRepositorioFacturacion repositorio)
        {
            RepositorioFacturacion = repositorio;
        }
        public IActionResult VentanaPrincipal()
        {
            List<DetalleFactura> ListadetalleFacturas = new List<DetalleFactura>();
            return View(ListadetalleFacturas);
        }

        public JsonResult SeleccionarProducto(string CodigoProducto)
        {
            return Json(RepositorioFacturacion.ObternerPorCodigo(CodigoProducto));
        }

        public JsonResult SeleccionarPersona(string cedulapersona)
        {
            return Json(RepositorioFacturacion.ObtenerPersonaPorCedula(cedulapersona));
        }

        [HttpPost]
        public ActionResult Facturaracion(double SubTotal, int Descuento, double Total,  string IdentificacionCliente, List<DetalleFactura> ListaProductos)
        {
            try
            {
                RepositorioFacturacion.Facturar(SubTotal, Descuento, Total, IdentificacionCliente, ListaProductos);
                return Json("La compra se ha efectuado con exito");
            }
            catch (Exception e)
            {
                return Json("Error inesperado, verifique los datos");
            }
        }
    }
}
