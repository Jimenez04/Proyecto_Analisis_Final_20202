using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Analisis_Final_20202.BL;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Analisis_Final_20202.Test.PersonaBLTest
{
    [TestClass]
    public class PersonaTest:BaseDatosFalsa
    {
        [TestMethod]
        public async Task ObtenerGeneros()
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Sexo.Add(new Sexo() { Nombre_Sexo = "Hombre", ID_Sexo = 1 });

            contexto.Sexo.Add(new Sexo() { Nombre_Sexo = "Mujer", ID_Sexo = 2 });

            await contexto.SaveChangesAsync();

            var persona = new PersonaBL(contexto);

            Assert.AreEqual(2, persona.ListadoDeSexos().Count);
        }

        [TestMethod]
        public async Task ObtenerTodasLasPersonas()
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Correo_Electronico.Add(new Correo_Electronico() { Cedula = "504250352", Correo = "jose.040199@hotmail.com" });
            contexto.Telefono.Add(new Telefono() { Cedula = "504250352", Numero = "88888888" });
            contexto.Persona.Add(new Persona() { Cedula = "504250352", Nombre1 = "Jose", Nombre2 = "Enrique", Apellido1 = "Jimenez" , Apellido2 = "Soto"});

            contexto.Correo_Electronico.Add(new Correo_Electronico() { Cedula = "504360671", Correo = "josue-1231@hotmailcom" });
            contexto.Telefono.Add(new Telefono() { Cedula = "504360671", Numero = "88888888" });
            contexto.Persona.Add(new Persona() { Cedula = "504360671", Nombre1 = "Josue", Nombre2 = "Israel", Apellido1 = "Machado", Apellido2 = "Velasquez" });

            contexto.Correo_Electronico.Add(new Correo_Electronico() { Cedula = "504190867", Correo = "sandymarif1297@gmailcom" });
            contexto.Telefono.Add(new Telefono() { Cedula = "504190867", Numero = "88888888" });
            contexto.Persona.Add(new Persona() { Cedula = "504190867", Nombre1 = "Maria", Nombre2 = "Fernanda", Apellido1 = "Sandi", Apellido2 = "Calderon" });

            await contexto.SaveChangesAsync();

            var persona = new PersonaBL(contexto);

            Assert.AreEqual("88888888", persona.ListarPersonas().First().telefono.Numero);

            Assert.AreEqual(3, persona.ListarPersonas().Count);

        }

        [TestMethod]
        public async Task AgregarPersona() //Se prueba el agregar persona y los metodos de buscar persona por cedula y por objeto
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            Correo_Electronico Correo = new Correo_Electronico() { Cedula = "504250352", Correo = "jose.040199@hotmail.com" };
            Telefono numero = new Telefono() { Cedula = "504250352", Numero = "88888888" };
            Persona nuevapersona  = new Persona() { Cedula = "504250352", Nombre1 = "Jose", Nombre2 = "Enrique", Apellido1 = "Jimenez", Apellido2 = "Soto" };

            nuevapersona.telefono = numero;
            nuevapersona.Correo = Correo;

            await contexto.SaveChangesAsync();

            var persona = new PersonaBL(contexto);

            persona.AgregarPersonas(nuevapersona);

            Assert.AreEqual("504250352", persona.ObtenerPersonaPorCedula("504250352").Cedula);

            Assert.IsTrue(persona.PersonaExiste(nuevapersona));

            Assert.AreEqual(1, persona.ListarPersonas().Count);

        }

        [TestMethod]
        public async Task EditarPersona() //Se prueba la correcta edicion de una persona
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            Correo_Electronico Correo = new Correo_Electronico() { Cedula = "504250352", Correo = "jose.040199@hotmail.com" };
            Telefono numero = new Telefono() { Cedula = "504250352", Numero = "88888888" };
            Persona nuevapersona = new Persona() { Cedula = "504250352", Nombre1 = "Jose", Nombre2 = "Enrique", Apellido1 = "Jimenez", Apellido2 = "Soto" };

            nuevapersona.telefono = numero;
            nuevapersona.Correo = Correo;

            await contexto.SaveChangesAsync();

            var persona = new PersonaBL(contexto);

            persona.AgregarPersonas(nuevapersona);

            Assert.AreEqual("Jose", persona.ObtenerPersonaPorCedula("504250352").Nombre1);

            Assert.IsTrue(persona.PersonaExiste(nuevapersona));

            nuevapersona.Nombre1 = "Fernanda";

            persona.EditarPersona(nuevapersona);

            Assert.AreEqual("Fernanda", persona.ObtenerPersonaPorCedula("504250352").Nombre1);

            Assert.IsTrue(persona.PersonaExiste(nuevapersona));

        }

        [TestMethod]
        public async Task ObtenerClliente() //Se prueba el metodo de obtenercliente por consecutivo, ademas el correcto funcionamiento de sus datos.
        {
            var nombre = Guid.NewGuid().ToString();

            var contexto = CrearContexto(nombre);

            contexto.Correo_Electronico.Add(new Correo_Electronico() { Cedula = "504250352", Correo = "jose.040199@hotmail.com" });
            contexto.Telefono.Add(new Telefono() { Cedula = "504250352", Numero = "88888888" });
            contexto.Persona.Add(new Persona() { Cedula = "504250352", Nombre1 = "Jose", Nombre2 = "Enrique", Apellido1 = "Jimenez", Apellido2 = "Soto" });

            contexto.Cliente.Add(new Cliente() { Consecutivo = "12345678901234567890", Cedula = "504250352", Descuento = 0 });

            await contexto.SaveChangesAsync();

            var persona = new PersonaBL(contexto);

            Cliente cliente = persona.ObtenerCliente_porConsecutivo("12345678901234567890");

            Assert.AreEqual("504250352", cliente.Cedula);

            Assert.AreEqual("Jose", persona.ObtenerPersonaPorCedula("504250352").Nombre1);
        }
    }
}
