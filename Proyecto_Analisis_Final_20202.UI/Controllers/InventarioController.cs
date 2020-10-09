﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        private readonly IRepositorioFacturacion RepositorioFacturacion;

        public InventarioController(IRepositorioFacturacion repositorio)
        {
            RepositorioFacturacion = repositorio;
        }

        [Authorize(Roles = "Administrador, Empleado")]
        [Route("Inventario/ListarProductosDisponibles")]
        public ActionResult ListarProductosDisponibles()
        {
            List<Inventario> LaListaDisponible;
            LaListaDisponible = RepositorioFacturacion.ObtenerProductosDisponibles();
            return View(LaListaDisponible);
        }

        [Authorize(Roles = "Administrador, Empleado")]
        [Route("Inventario/ListarProductosSinExistencia")]
        public ActionResult ListarProductosSinExistencia()
        {
            List<Inventario> LaListaSinExistencia;
            LaListaSinExistencia = RepositorioFacturacion.ObtenerProductosSinExistencia();
            return View(LaListaSinExistencia);
        }

        [Authorize(Roles = "Administrador, Empleado")]
        [Route("Inventario/ListarInventario")]
        public ActionResult ListarInventario()
        {
            List<Inventario> LaLista;
            LaLista = RepositorioFacturacion.ListaInventario();
            return View(LaLista);
        }

        [Authorize(Roles = "Administrador")]
        [Route("Inventario/AgregarInventario")]
        public ActionResult AgregarInventario(String Accion)
        {
            ViewBag.Accion = Accion;
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [Route("Inventario/AgregarInventario")]
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
                    else
                    {
                        ModelState.AddModelError("Codigo_Prodcuto", "El código ingresado  ya  existe");
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
        [Authorize(Roles = "Administrador")]
        [Route("Inventario/AgregarInventarioDisponible")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarInventarioDisponible(Inventario inventario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!RepositorioFacturacion.ProductoExiste(inventario))
                    {
                        RepositorioFacturacion.AgregarInventario(inventario);
                        return RedirectToAction(nameof(ListarProductosDisponibles));
                    }
                    else
                    {
                        ModelState.AddModelError("Codigo_Prodcuto", "El código ingresado  ya  existe");
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
        [Authorize(Roles = "Administrador")]
        [Route("Inventario/EditarProducto")]
        public ActionResult EditarProducto(String Codigo_Prodcuto, String Accion)
        {
            Inventario producto = RepositorioFacturacion.ObternerPorCodigo(Codigo_Prodcuto);
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("Inventario/EditarProducto")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(Inventario producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarProducto(producto);
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
        //
        [Authorize(Roles = "Administrador")]
        [Route("Inventario/EditarProductoDisponible")]
        public ActionResult EditarProductoDisponible(String Codigo_Prodcuto)
        {
            Inventario producto = RepositorioFacturacion.ObternerPorCodigo(Codigo_Prodcuto);
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("Inventario/EditarProductoDisponible")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProductoDisponible(Inventario producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarProducto(producto);
                    return RedirectToAction(nameof(ListarProductosDisponibles));
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
        //
        [Authorize(Roles = "Administrador")]
        [Route("Inventario/EditarProductoFueraDeInventario")]
        public ActionResult EditarProductoFueraDeInventario(String Codigo_Prodcuto, String Accion)
        {
            Inventario producto = RepositorioFacturacion.ObternerPorCodigo(Codigo_Prodcuto);
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("Inventario/EditarProductoFueraDeInventario")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProductoFueraDeInventario(Inventario producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositorioFacturacion.EditarProducto(producto);
                    return RedirectToAction(nameof(ListarProductosSinExistencia));
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

        [Authorize(Roles = "Administrador")]
        [Route("Inventario/EliminarProducto")]
        public ActionResult EliminarProducto(string Codigo_Prodcuto, string Accion)
        {
            try
            {
                RepositorioFacturacion.FueraServicio(Codigo_Prodcuto);
                if (Accion.Equals("Total")) 
                {
                    return RedirectToAction(nameof(ListarInventario));
                }else
                {
                    return RedirectToAction(nameof(ListarProductosDisponibles));
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(ListarInventario));
            }
        }
    }
}
