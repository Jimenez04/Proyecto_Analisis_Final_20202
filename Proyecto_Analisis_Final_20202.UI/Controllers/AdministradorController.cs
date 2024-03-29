﻿using System;
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
        private readonly EmpresaBL RepositorioEmpresa;
        private readonly UbicacionBL RepositorioUbicacion;

        public AdministradorController(EmpresaBL empresa , UbicacionBL ubicacionBL)
        {
            this.RepositorioEmpresa = empresa;
            this.RepositorioUbicacion = ubicacionBL;
        }


        public ActionResult ListarUsuarios()
        {
            return View();
        }

        public ActionResult EditarEmpresa()
        {
            Empresa empresa = this.RepositorioEmpresa.ObtenerEmpresa();
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones((int)empresa.ID_Provincia), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos((int)empresa.ID_Provincia, (int)empresa.ID_Canton), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEmpresa(Empresa empresa)
        {
            ViewBag.Pais = new SelectList(RepositorioUbicacion.ListadoDeProvincias(), "ID_Provincia", "Nombre_Provincia");
            ViewBag.Cantones = new SelectList(RepositorioUbicacion.ListadoDeCantones((int)empresa.ID_Provincia), "ID_Canton", "ID_Provincia", "Nombre_Canton");
            ViewBag.Distritos = new SelectList(RepositorioUbicacion.ListadoDeDistritos((int)empresa.ID_Provincia, (int)empresa.ID_Canton), "ID_Distrito", "ID_Canton", "ID_Provincia", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    this.RepositorioEmpresa.EditarEmpresa(empresa);
                    return RedirectToAction("ListarPersonas", "Persona");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }
    }    
}
