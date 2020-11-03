using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Analisis_Final_20202.BL
{
    public class PersonaBL
    {
        private ContextoBaseDeDatos ElContextoDeBaseDeDatos;
        public PersonaBL(ContextoBaseDeDatos contexto)
        {
            ElContextoDeBaseDeDatos = contexto;
        }

        public List<Persona> ListarPersonas()
        {
            List<Persona> laListaDePersonas;
            laListaDePersonas = ElContextoDeBaseDeDatos.Persona.ToList();
            foreach (var item in laListaDePersonas)
            {
                item.telefono = ElContextoDeBaseDeDatos.Telefono.Find(item.Cedula);
                item.Correo = ElContextoDeBaseDeDatos.Correo_Electronico.Find(item.Cedula);
            }
            return laListaDePersonas;
        }

        public void AgregarPersonas(Persona persona)
        {
            try
            {
                persona.telefono.Cedula = persona.Cedula;
                persona.Correo.Cedula = persona.Cedula;


                ElContextoDeBaseDeDatos.Persona.Add(persona);
                ElContextoDeBaseDeDatos.SaveChanges();

                ElContextoDeBaseDeDatos.Telefono.Add(persona.telefono);
                ElContextoDeBaseDeDatos.SaveChanges();

                ElContextoDeBaseDeDatos.Correo_Electronico.Add(persona.Correo);
                ElContextoDeBaseDeDatos.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //  ElContextoDeBaseDeDatos.RollBack();

                throw;
            }


        }

        public bool PersonaExiste(Persona persona)
        {

            var existencia = ElContextoDeBaseDeDatos.Persona.Find(persona.Cedula);

            if (existencia != null)
            {

                return true;

            }
            else
            {
                return false;

            }
        }

        public Persona ObtenerPersonaPorCedula(String Cedula)
        {
            try
            {
                Persona persona;
                persona = ElContextoDeBaseDeDatos.Persona.Find(Cedula);
                persona.Correo = ElContextoDeBaseDeDatos.Correo_Electronico.Find(Cedula);
                persona.telefono = ElContextoDeBaseDeDatos.Telefono.Find(Cedula);
                return persona;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public void EditarPersona(Persona persona)
        {
            ElContextoDeBaseDeDatos.Persona.Update(persona);
            ElContextoDeBaseDeDatos.SaveChanges();

        }

        public Cliente ObtenerCliente_porConsecutivo(string consecutivo)
        {
            return (from c in ElContextoDeBaseDeDatos.Cliente
                    where c.Consecutivo == consecutivo
                    select c).First();
        }

        public List<Sexo> ListadoDeSexos()
        {
            return ElContextoDeBaseDeDatos.Sexo.ToList();
        }
    }
}
