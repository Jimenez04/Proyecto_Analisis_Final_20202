using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    [Authorize(Roles = "Administrador, Empleado")]
    public class PersonaController : Controller
    {
        private readonly IRepositorioFacturacion RepositorioFacturacion;

        public PersonaController(IRepositorioFacturacion repositorio)
        {
            RepositorioFacturacion = repositorio;
        }

        public ActionResult ListarPersonas()
        {
            List<Persona> LaLista;
            LaLista = RepositorioFacturacion.ListarPersonas();
            return View(LaLista);
        }
        [Route("Persona/AgregarPersona")]
        public ActionResult AgregarPersona()
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            return View();
        }

        [HttpPost]
        [Route("Persona/AgregarPersona")]
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
                    if (!RepositorioFacturacion.PersonaExiste(persona))
                    {
                        RepositorioFacturacion.AgregarPersonas(persona);
                        return RedirectToAction(nameof(ListarPersonas));
                    }
                    else
                    {
                        ModelState.AddModelError("Cedula", "La cédula ingresada ya  existe en nuestro sistema");
                        return View();
                    }
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
        [Route("Persona/EditarPersona")]
        public ActionResult EditarPersona(String Cedula)
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            Persona persona = RepositorioFacturacion.ObtenerPersonaPorCedula(Cedula);
            return View(persona);
        }

        [HttpPost]
        [Route("Persona/EditarPersona")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPersona(Persona persona)
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarPersona(persona);
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
    }
}
