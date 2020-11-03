using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Test.EmpresaBLTest
{
    [TestClass]
    public class EmpresaTest:BaseDatosFalsa
    {
        [TestMethod]
        public async Task VerificarExistenciaEmpresa()  //Verificar que una empresa existe y obtener empresa
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            Empresa crearempresa = new Empresa()
            {
               Cedula_Juridica = "1234567890",
               Nombre = "Patitos S.A",
               Razon_Social = "Ayuda infantil",
            };

            contexto.Empresa.Add(crearempresa);
            await contexto.SaveChangesAsync();

            var repositorioEmpresaBL = new EmpresaBL(contexto);

            Assert.IsTrue(repositorioEmpresaBL.VerificaciondeExistenciaEmpresa("1234567890"));
            Assert.AreEqual("Patitos S.A", repositorioEmpresaBL.ObtenerEmpresa().Nombre);
        }

        [TestMethod]
        public async Task EditarEmpresa()  //Edicion de nombre de una empresa
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            Empresa crearempresa = new Empresa()
            {
                Cedula_Juridica = "1234567890",
                Nombre = "Patitos S.A",
                Razon_Social = "Ayuda infantil",
            };

            contexto.Empresa.Add(crearempresa);
            await contexto.SaveChangesAsync();

            var repositorioEmpresaBL = new EmpresaBL(contexto);

            Assert.AreEqual("Patitos S.A", repositorioEmpresaBL.ObtenerEmpresa().Nombre);
            crearempresa.Nombre = "Costa Rica";
            contexto.Empresa.Add(crearempresa);
            Assert.AreEqual("Costa Rica", repositorioEmpresaBL.ObtenerEmpresa().Nombre);

        }

    }
}
