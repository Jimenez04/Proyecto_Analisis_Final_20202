using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto_Analisis_Final_20202.BL
{
    public class RepositorioFacturacion : IRepositorioFacturacion
    {
        private ContextoBaseDeDatos ElContextoDeBaseDeDatos;
        public RepositorioFacturacion(ContextoBaseDeDatos contexto)
        {
            ElContextoDeBaseDeDatos = contexto;
        }

        public List<Canton> ListadoDeCantones(int ID_Provincia)
        {
            return (from c in ElContextoDeBaseDeDatos.Canton
                    where (c.ID_Provincia == ID_Provincia)
                    select c).ToList(); 
        }

        public List<Distrito> ListadoDeDistritos(int ID_Provincia, int ID_Canton)
        {
            return (from c in ElContextoDeBaseDeDatos.Distrito
                    where (c.ID_Canton == ID_Provincia)
                    && (c.ID_Provincia == ID_Canton)
                    select c).ToList();
        }

        public List<Provincia> ListadoDeProvincias()
        {
            return ElContextoDeBaseDeDatos.Provincia.ToList();
        }

        public List<Sexo> ListadoDeSexos()
        {
            return ElContextoDeBaseDeDatos.Sexo.ToList();
        }
        public bool VerificaciondeExistenciaEmpresa(string Cedula_Judica)
        {
          Empresa empresa = new Empresa();
            empresa = ElContextoDeBaseDeDatos.Empresa.Find(Cedula_Judica);
           
            if (empresa != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CreacionDeCuentaEmpresarial(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial)
        {
            Empresa NuevaEmpresa = new Empresa();
            NuevaEmpresa.Cedula_Juridica = nuevaCuentaEmpresarial.Cedula;
            NuevaEmpresa.Nombre = nuevaCuentaEmpresarial.NombreOrganizacion;
           // EnvioDeDatosParaLoginCorreo(usuario_Empresa.Clave, usuario_Empresa.Nombre_Usuario, nuevaCuentaEmpresarial.CorreoElectronico);
            //InicioSecion(usuario_Empresa.Clave, usuario_Empresa.Nombre_Usuario);

            //-------  Datos de Invento Ficticios----------------- 

            NuevaEmpresa.Razon_Social = "Ingrese su razón";
            NuevaEmpresa.ID_Provincia = 1;
            NuevaEmpresa.ID_Canton = 1;
            NuevaEmpresa.ID_Distrito = 1;
            NuevaEmpresa.Senas_Exactas = "Ingrese sus señas";
            NuevaEmpresa.Codigo_Postal = "10101";

            //------- Termina la mamadera ----------------- 

            ElContextoDeBaseDeDatos.Empresa.Add(NuevaEmpresa);
            ElContextoDeBaseDeDatos.SaveChanges();

            /* if (false) 
             {

                 return true;
             }
             else
             {
                 return false;
             }
            */
            return true;
        }

        public string GeneradorDeContrasena()
        {
            Random GeneradorRandom = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int longitud = caracteres.Length;
            char caracter;
            int LargoContrasena = 3;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < LargoContrasena; i++)
            {
                caracter = caracteres[GeneradorRandom.Next(longitud)];
                contraseniaAleatoria += caracter.ToString();
            }
            caracteres = "1234567890";
            longitud = caracteres.Length;
            LargoContrasena = 4;
            for (int i = 0; i < LargoContrasena; i++)
            {
                caracter = caracteres[GeneradorRandom.Next(longitud)];
                contraseniaAleatoria += caracter.ToString();
            }
            return contraseniaAleatoria;
        }

        //Metodos para el inventario
        public List<Inventario> ListaInventario()
        {
            List<Inventario> laListaDeInventario;
            laListaDeInventario = ElContextoDeBaseDeDatos.Inventario.ToList();
            return laListaDeInventario;
        }

        public Inventario ObternerPorCodigo(String codigo)
        {
            Inventario producto;
            producto = ElContextoDeBaseDeDatos.Inventario.Find(codigo);
            return producto;
        }

        public void AgregarInventario(Inventario inventario)
        {
            inventario.ID_Estado = 1;
            inventario.ID_Categoria = 1;
            ElContextoDeBaseDeDatos.Inventario.Add(inventario);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public void EditarProducto(Inventario producto)
        {
            ElContextoDeBaseDeDatos.Inventario.Update(producto);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public List<Persona> ListarPersonas()
        {
            List<Persona> laListaDePersonas;
            laListaDePersonas = ElContextoDeBaseDeDatos.Persona.ToList();
            return laListaDePersonas;
        }

        public void AgregarPersonas(Persona persona)
        {
            Correo_Electronico correopersona = new Correo_Electronico();
            Telefono telefono = new Models.Telefono();

            telefono.Cedula = persona.Cedula;
            correopersona.Cedula= persona.Cedula;
            telefono.Numero = persona.NumeroTelefonico;
            correopersona.Correo = persona.CorreoElectronico;

            ElContextoDeBaseDeDatos.Persona.Add(persona);
            ElContextoDeBaseDeDatos.SaveChanges();
            ElContextoDeBaseDeDatos.Telefono.Add(telefono);
            ElContextoDeBaseDeDatos.Correo_Electronico.Add(correopersona);
            ElContextoDeBaseDeDatos.SaveChanges();
        }

        public bool PersonaExiste(Persona persona)
        {
            
            var existencia = ElContextoDeBaseDeDatos.Persona.Find(persona.Cedula);

            if (existencia != null) {

                return true;

            } else
            {
                return false;

            }                 
        }

        public bool ProductoExiste( Inventario producto)
        {

            var existencia = ElContextoDeBaseDeDatos.Inventario.Find(producto.Codigo_Prodcuto);

            if (existencia != null)
            {

                return true;

            }
            else
            {
                return false;

            }
        }
    }
}
