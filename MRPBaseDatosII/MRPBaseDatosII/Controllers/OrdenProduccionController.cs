using Microsoft.AspNetCore.Mvc;
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
            var ordenesProduccion = await repositorioOrdenProduccion.ObtenerOrdenProduccion();
            return View(ordenesProduccion);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
