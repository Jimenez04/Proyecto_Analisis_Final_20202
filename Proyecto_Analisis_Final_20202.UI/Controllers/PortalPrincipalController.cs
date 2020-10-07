using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public PortalPrincipalController(UserManager<IdentityUser> userManager, IRepositorioFacturacion repositorio, 
            SignInManager<IdentityUser> Log, RoleManager<IdentityRole> Rol, IEmailSender emailSender)
        {
            gestionusuarios = userManager;
            RepositorioFacturacion = repositorio;
            Roles = Rol;
            Login = Log;
            _emailSender = emailSender;
        }

      //  [Route("PortalPrincipalController/PantallaPrincipal")]
      [AllowAnonymous]
        public ActionResult PantallaPrincipal()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Administrador"))
            {
                Response.Redirect("/Administrador/ListarPersonas");
            }
            else if (User.Identity.IsAuthenticated && User.IsInRole("Empleado"))
            {
                Response.Redirect("/Usuario/VentanaPrincipal");
            } else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("PortalPrincipal/PantallaPrincipal")]
        public async Task<ActionResult> PantallaPrincipal(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial)
        {
            if (ModelState.IsValid) 
            {
                if (RepositorioFacturacion.VerificaciondeExistenciaEmpresa(nuevaCuentaEmpresarial.Cedula)) //Quitar esto
                {
                    string contrasena = RepositorioFacturacion.GeneradorDeContrasena();
                    var Usuario = new IdentityUser
                    {
                        UserName = nuevaCuentaEmpresarial.NombreCompleto,
                        Email = nuevaCuentaEmpresarial.CorreoElectronico,
                        PhoneNumber = nuevaCuentaEmpresarial.NumeroTelefonico,
                    };
                    var Estado = await gestionusuarios.CreateAsync(Usuario, contrasena);
                    await gestionusuarios.AddToRoleAsync(Usuario, "Administrador");
                    if (Estado.Succeeded)
                    {
                        await Login.SignInAsync(Usuario, isPersistent: false);
                        await _emailSender.SendEmailAsync(Usuario.Email, "Creación de nuevo usuario administrador",
                     "Usuario " + Usuario.UserName + ", contraseña temporal: " + contrasena + ", esta contraseña es su responsabilidad, cámbiela lo antes posible" );
                        return RedirectToAction("PantallaPrincipal", "PortalPrincipal");
                    }
                    else
                    {
                        return View();
                    }
                }
                else 
                {
                    ModelState.AddModelError("Cedula", "La empresa ingresada no  existe en nuestro sistema");
                }
            }
            return View();
        }
    }
}
