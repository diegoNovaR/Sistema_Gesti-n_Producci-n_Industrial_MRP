using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class OrdenCompraController : Controller
    {
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        public OrdenCompraController(IRepositorioMateriaPrima repositorioMateriaPrima)
        {
            this.repositorioMateriaPrima = repositorioMateriaPrima;
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(OrdenCompra orden)
        {
            //aqui se puede agregar validaciones si es necesario MAS ADELANTE 

            await repositorioMateriaPrima.Crear(orden);
            return View("Index");//devolver a la vista principal o a otra vista que se configure
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
