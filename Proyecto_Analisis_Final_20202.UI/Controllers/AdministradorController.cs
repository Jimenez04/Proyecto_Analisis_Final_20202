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

        public ActionResult ListarInventario()
        {
            List<Inventario> LaLista;
            LaLista = RepositorioFacturacion.ListaInventario();
            return View(LaLista);
        }

        public ActionResult ListarEmpresa()
        {
            List<Empresa> LaLista;
            LaLista = RepositorioFacturacion.ListarEmpresa();
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
                    if (!RepositorioFacturacion.ProductoExiste(inventario))
                    {
                        RepositorioFacturacion.AgregarInventario(inventario);
                        return RedirectToAction(nameof(ListarInventario));

                    }
                    else {

                        ModelState.AddModelError("Codigo_Prodcuto", "El código de producto ingresado ya  existe en nuestro sistema");

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







        public ActionResult EditarProducto(String Codigo_Prodcuto)
        {
     

            Inventario producto = RepositorioFacturacion.ObternerPorCodigo(Codigo_Prodcuto);
            return View(producto);
        }

        // POST: AdministradorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(Inventario producto)
        {
            

            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarProducto(producto);

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

            return View();
        }

        public ActionResult EditarPersona(String Cedula)
        {
            ViewBag.Pais = new SelectList(RepositorioFacturacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioFacturacion.ListadoDeCantones(0), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioFacturacion.ListadoDeDistritos(0, 0), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            ViewBag.Sexo = new SelectList(RepositorioFacturacion.ListadoDeSexos(), "ID_Sexo", "Nombre_Sexo");
            Persona persona = RepositorioFacturacion.ObtenerPersonaPorCedula(Cedula);
            return View(persona);
        }

        // POST: AdministradorController/Create
        [HttpPost]
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

            return View();
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

            return View();
        }



       
       

    }
}
