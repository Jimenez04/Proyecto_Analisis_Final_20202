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
