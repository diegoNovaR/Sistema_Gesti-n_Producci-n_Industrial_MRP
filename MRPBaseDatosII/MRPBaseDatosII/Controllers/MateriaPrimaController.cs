using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;
using System.Security.Cryptography.X509Certificates;

namespace MRPBaseDatosII.Controllers
{
    public class MateriaPrimaController : Controller
    {
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        public MateriaPrimaController(IRepositorioMateriaPrima repositorioMateriaPrima)
        {
            this.repositorioMateriaPrima = repositorioMateriaPrima;
        }


        public async Task<IActionResult> Crear()//accion para mostrar la vista de crear materia prima(formulario)
        {
            var modelo = new MateriaPrima();//creamos un objeto materia prima vacio
            return View(modelo);//vista sin objeto
        }


        [HttpPost]
        public async Task<IActionResult> Crear(MateriaPrima materiaPrima)//recibimos el objeto materia prima desde la vista
        {
            var materia = await repositorioMateriaPrima.CrearMateriaPrima(materiaPrima);//guardamos el objeto materia prima pasado como parametro
            
            var materiaListado = await repositorioMateriaPrima.ObtenerListadoMateriaPrima();
            return View("Index",materiaListado);
        }

        public async Task<IActionResult> Index()
        {
            var materiaPrima = await repositorioMateriaPrima.ObtenerListadoMateriaPrima();
            return View(materiaPrima);//la vista recibe el listado de materia prima
        }
    }
}
