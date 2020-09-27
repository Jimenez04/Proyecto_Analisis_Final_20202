using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult VentanaPrincipal()
        {
            List<DetalleFactura> ListadetalleFacturas = new List<DetalleFactura>();
            return View(ListadetalleFacturas);
        }
    }
}
