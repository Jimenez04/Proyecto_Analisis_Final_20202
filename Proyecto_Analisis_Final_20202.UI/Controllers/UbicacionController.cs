
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    public class UbicacionController : Controller
    {
        private readonly UbicacionBL RepositorioUbicacion;

        public UbicacionController(UbicacionBL ubicacion)
        {this.RepositorioUbicacion = ubicacion;

        }
        public JsonResult RecargarCanton(int ID_Provincia)
        {
            List<Canton> Cantones = RepositorioUbicacion.ListadoDeCantones(ID_Provincia);

            ViewBag.Cantones = new SelectList(Cantones, "ID_Canton", "ID_Provincia", "Nombre_Canton");

            return Json(Cantones);
        }

        public JsonResult RecargarDistrito(String Ubicacion)
        {
            string[] Coordenadas = Ubicacion.Split(new Char[] { '/' });
            List<Distrito> Distritos = new List<Distrito>();
            if (!Coordenadas.Contains(""))
            {
                int ID_Provincia = int.Parse(Coordenadas[0].Trim());
                int ID_Canton = int.Parse(Coordenadas[1].Trim());
                Distritos = RepositorioUbicacion.ListadoDeDistritos(ID_Provincia, ID_Canton);
            }
            ViewBag.Distritos = new SelectList(Distritos, "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            return Json(Distritos);
        }
    }
}
