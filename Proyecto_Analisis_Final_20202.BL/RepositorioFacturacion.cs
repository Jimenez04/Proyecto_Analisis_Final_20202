﻿using Proyecto_Analisis_Final_20202.DA;
using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}