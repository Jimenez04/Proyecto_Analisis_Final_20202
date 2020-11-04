using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Test.UbicacionBLTest
{
    [TestClass]
    public class UbicacionTest:BaseDatosFalsa
    {

        [TestMethod]
        public async Task ObtenerProvincias()
        {
            var nombre = Guid.NewGuid().ToString();
            var contexto = CrearContexto(nombre);

            contexto.Provincia.Add(new Provincia() { Nombre_Provincia = "Guanacaste", ID_Provincia = 5 });
            contexto.Provincia.Add(new Provincia() { Nombre_Provincia = "Cartago", ID_Provincia = 3 });
            contexto.Provincia.Add(new Provincia() { Nombre_Provincia = "San Jose", ID_Provincia = 1 });

            await contexto.SaveChangesAsync();
            var ubicacion = new UbicacionBL(contexto);

            Assert.AreEqual(3, ubicacion.ListadoDeProvincias().Count);
            Assert.AreEqual("Guanacaste", ubicacion.ListadoDeProvincias().First().Nombre_Provincia);
        }

        [TestMethod]
        public async Task ObtenerCantonesPorProvincia()
        {
            var nombre = Guid.NewGuid().ToString();
            var contexto = CrearContexto(nombre);

            contexto.Provincia.Add(new Provincia() { Nombre_Provincia = "Guanacaste", ID_Provincia = 5 });
            
            contexto.Canton.Add(new Canton() { Nombre_Canton = "Carrillo", ID_Canton = 5 , ID_Provincia = 5 });
            contexto.Canton.Add(new Canton() { Nombre_Canton = "Santa Cruz", ID_Canton = 3 , ID_Provincia = 5 });

            await contexto.SaveChangesAsync();
            var ubicacion = new UbicacionBL(contexto);

            Assert.AreEqual(2, ubicacion.ListadoDeCantones(5).Count);
            Assert.AreEqual("Carrillo", ubicacion.ListadoDeCantones(5).First().Nombre_Canton);
        }

        [TestMethod]
        public async Task ObtenerDistritosPorCanton()
        {
            var nombre = Guid.NewGuid().ToString();
            var contexto = CrearContexto(nombre);

            contexto.Provincia.Add(new Provincia() { Nombre_Provincia = "Guanacaste", ID_Provincia = 5 });

            contexto.Canton.Add(new Canton() { Nombre_Canton = "Carrillo", ID_Canton = 5, ID_Provincia = 5 });

            contexto.Distrito.Add(new Distrito() { Nombre = "Sardinal", ID_Distrito = 3 ,ID_Canton = 5, ID_Provincia = 5 });
            contexto.Distrito.Add(new Distrito() { Nombre = "Filadelfia", ID_Distrito = 1, ID_Canton = 5, ID_Provincia = 5 });

            await contexto.SaveChangesAsync();
            var ubicacion = new UbicacionBL(contexto);

            Assert.AreEqual(2, ubicacion.ListadoDeDistritos(5 , 5).Count);
            Assert.AreEqual("Sardinal", ubicacion.ListadoDeDistritos(5, 5).First().Nombre);
        }
    }
}
