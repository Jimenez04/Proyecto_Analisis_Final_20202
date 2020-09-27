using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    public class PortalPrincipalController : Controller
    {
        public ActionResult PantallaPrincipal()
        {
            return View();
        }
    }
}
