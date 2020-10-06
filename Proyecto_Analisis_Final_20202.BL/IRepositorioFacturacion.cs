using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Analisis_Final_20202.BL
{
   public interface IRepositorioFacturacion
    {
        public Boolean VerificaciondeExistenciaEmpresa(string cedulaJudica);
        public List<Sexo> ListadoDeSexos();

        public List<Provincia> ListadoDeProvincias();

        public List<Canton> ListadoDeCantones(int ID_Provincia);

        public List<Distrito> ListadoDeDistritos(int ID_Provincia, int ID_Canton);

        public String GeneradorDeContrasena();  

            

        public Boolean CreacionDeCuentaEmpresarial(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial);

        //Metodos para Administrador

        public List<Inventario> ListaInventario();

        public Inventario ObternerPorCodigo(String codigo);

        public Persona ObtenerPersonaPorCedula(String Cedula);

        public void EditarProducto(Inventario producto);

         public void EditarPersona(Persona persona);

        public Boolean PersonaExiste(Persona persona);


        public Boolean ProductoExiste(Inventario producto);

        public void AgregarInventario(Inventario inventario);

        public List<Persona> ListarPersonas();

        public void AgregarPersonas(Persona persona);

    }
}
