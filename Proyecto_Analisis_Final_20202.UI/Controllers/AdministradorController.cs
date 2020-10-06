using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ActionResult ListarInventario()
        {
            List<Inventario> LaLista;
            LaLista = RepositorioFacturacion.ListaInventario();
            return View(LaLista);
        }

        public ActionResult ListarPersonas()
        {
            List<Persona> LaLista;
            LaLista = RepositorioFacturacion.ListarPersonas();
            return View(LaLista);
        }

        // GET: AdministradorController/Create
        public ActionResult AgregarInventario()
        {
            return View();
        }

        // POST: AdministradorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarInventario(Inventario inventario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.AgregarInventario(inventario);
                    return RedirectToAction(nameof(ListarInventario));
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

        public ActionResult AgregarPersona()
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            return View();
        }

        // POST: AdministradorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPersona(Persona persona)
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.AgregarPersonas(persona);
                    return RedirectToAction(nameof(ListarPersonas));
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







        // GET: AdministradorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdministradorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
       
    }
}
