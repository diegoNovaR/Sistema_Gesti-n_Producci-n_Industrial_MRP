using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class OrdenProduccionController : Controller
    {
        private readonly IRepositorioOrdenProduccion repositorioOrdenProduccion;
        public OrdenProduccionController(IRepositorioOrdenProduccion repositorioOrdenProduccion)
        {
            this.repositorioOrdenProduccion = repositorioOrdenProduccion;
        }
        public async Task<IActionResult> Crear()
        {
            var laptops = await repositorioOrdenProduccion.ObtenerOrdenProduccion();

            var modelo = new CrearOrdenProduccionViewModel
            {
                Laptops = laptops.ToList()
            };

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearOrdenProduccionViewModel ordenProduccionVM)
        {
            var ordenProduccionCreada = await repositorioOrdenProduccion.Crear(ordenProduccionVM);
            return RedirectToAction("Index",ordenProduccionCreada);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
