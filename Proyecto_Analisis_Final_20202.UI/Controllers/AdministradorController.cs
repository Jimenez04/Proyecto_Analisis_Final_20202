using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    [Authorize]
    public class AdministradorController : Controller
    {
        private readonly IRepositorioFacturacion RepositorioFacturacion;

        public AdministradorController(IRepositorioFacturacion repositorio)
        {
            RepositorioFacturacion = repositorio;
        }


        // GET: AdministradorController
        public ActionResult ListarUsuarios()
        {
            return View();
        }

        public ActionResult ListarEmpresa()
        {
            List<Empresa> LaLista;
            LaLista = RepositorioFacturacion.ListarEmpresa();
            return View(LaLista);
        }

        

        public ActionResult EditarEmpresa()
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");

            Empresa empresa = RepositorioFacturacion.ObtenerEmpresa();
            return View(empresa);
        }

        // POST: AdministradorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEmpresa(Empresa empresa)
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");

            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarEmpresa(empresa);
                    return RedirectToAction(nameof(ListarEmpresa));
                }
                else
                {
                    return View();
                }
            }
            catch
            {

                return View();
            }

           
        }

        // GET: AdministradorController/Create
      



    }    
}
