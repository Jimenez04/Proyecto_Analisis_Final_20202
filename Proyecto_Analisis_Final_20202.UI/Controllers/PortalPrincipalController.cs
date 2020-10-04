using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;

namespace Proyecto_Analisis_Final_20202.UI.Controllers
{
    [Authorize]
    public class PortalPrincipalController : Controller
    {
        private readonly UserManager<IdentityUser> gestionusuarios;
        private readonly IRepositorioFacturacion RepositorioFacturacion;
        private readonly SignInManager<IdentityUser> Login;
        private readonly RoleManager<IdentityRole> Roles;

        public PortalPrincipalController(UserManager<IdentityUser> userManager, IRepositorioFacturacion repositorio, 
            SignInManager<IdentityUser> Log, RoleManager<IdentityRole> Rol)
        {
            gestionusuarios = userManager;
            RepositorioFacturacion = repositorio;
            Roles = Rol;
            Login = Log;
        }

      //  [Route("PortalPrincipalController/PantallaPrincipal")]
      [AllowAnonymous]
        public ActionResult PantallaPrincipal()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("PortalPrincipal/PantallaPrincipal")]
        public async Task<ActionResult> PantallaPrincipal(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial)
        {
            if (ModelState.IsValid) 
            {
                if (!RepositorioFacturacion.VerificaciondeExistenciaEmpresa(nuevaCuentaEmpresarial.Cedula))
                {
                    string contrasena = RepositorioFacturacion.GeneradorDeContrasena();
                    var Usuario = new IdentityUser
                    {
                        UserName = nuevaCuentaEmpresarial.Cedula,
                        Email = nuevaCuentaEmpresarial.CorreoElectronico,
                    };
                    var Estado = await gestionusuarios.CreateAsync(Usuario, contrasena);
                    await gestionusuarios.AddToRoleAsync(Usuario, "Administrador");
                    if (Estado.Succeeded)
                    {
                        await Login.SignInAsync(Usuario, isPersistent: false);
                        return RedirectToAction("VentanaPrincipal", "Usuario");
                    }
                    else
                    {
                        return View();
                    }
                }
                else 
                {
                    ModelState.AddModelError("Cedula", "La cédula ingresada ya  existe en nuestro sistema");
                }
            }
            return View();
        }
    }
}
