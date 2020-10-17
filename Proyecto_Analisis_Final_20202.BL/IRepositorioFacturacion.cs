using Proyecto_Analisis_Final_20202.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Analisis_Final_20202.BL
{
   public interface IRepositorioFacturacion
    {
        public Boolean VerificaciondeExistenciaEmpresa(string cedulaJudica);

        public List<DetalleFactura> Carrito(String codigo);
        public Empresa ObtenerEmpresa();
        public List<Sexo> ListadoDeSexos();

        public List<Provincia> ListadoDeProvincias();

        public List<Canton> ListadoDeCantones(int ID_Provincia);

        public List<Distrito> ListadoDeDistritos(int ID_Provincia, int ID_Canton);

        public String GeneradorDeContrasena();  

            

        public Boolean CreacionDeCuentaEmpresarial(ModeloNuevaCuentaEmpresarial nuevaCuentaEmpresarial);

        //Metodos para Administrador

        public List<Inventario> ListaInventario();

        public Inventario ObternerPorCodigo(String codigo);

        //Para Inventario
        

        public void FueraServicio(string Codigo_Prodcuto);

        public Persona ObtenerPersonaPorCedula(String Cedula);

        public void EditarProducto(Inventario producto);

         public void EditarPersona(Persona persona);

        public Boolean PersonaExiste(Persona persona);

        public void EditarEmpresa(Empresa empresa);

        public List<Inventario> ObtenerProductosSinExistencia ();
        public List<Inventario> ObtenerProductosDisponibles();
        public Boolean ProductoExiste(Inventario producto);

        public void AgregarInventario(Inventario inventario);

        public List<Persona> ListarPersonas();

        public void AgregarPersonas(Persona persona);

    }
}
