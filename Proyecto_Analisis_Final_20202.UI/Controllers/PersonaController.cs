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
        
        private readonly PersonaBL RepositorioPersona;
        private readonly UbicacionBL RepositorioUbicacion;

        public PersonaController(PersonaBL Persona, UbicacionBL ubicacion)
        {
            this.RepositorioPersona = Persona;
            this.RepositorioUbicacion = ubicacion;
        }

        public ActionResult ListarPersonas()
        {
            List<Persona> LaLista;
            LaLista = RepositorioPersona.ListarPersonas();
            return View(LaLista);
        }
        [Route("RepositorioPersona/AgregarPersona")]
        public ActionResult AgregarPersona()
        {
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioPersona.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            return View();
        }

        [HttpPost]
        [Route("RepositorioPersona/AgregarPersona")]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPersona(Persona persona)
        {
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioPersona.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            try
            {
                if (ModelState.IsValid)
                {
                    if (!RepositorioPersona.PersonaExiste(persona))
                    {
                        RepositorioPersona.AgregarPersonas(persona);
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
            catch(Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
        [Route("RepositorioPersona/EditarPersona")]
        public ActionResult EditarPersona(String Cedula)
        {
            Persona persona = RepositorioPersona.ObtenerPersonaPorCedula(Cedula);
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones((int)persona.ID_Provincia), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos((int)persona.ID_Provincia, (int)persona.ID_Canton), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioPersona.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            return View(persona);
        }

        [HttpPost]
        [Route("RepositorioPersona/EditarPersona")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPersona(Persona persona)
        {
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioPersona.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioPersona.EditarPersona(persona);
                    return RedirectToAction(nameof(ListarPersonas));
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
    }
}
