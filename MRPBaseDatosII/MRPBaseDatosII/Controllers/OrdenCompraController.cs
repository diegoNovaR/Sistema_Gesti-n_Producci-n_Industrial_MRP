using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class OrdenCompraController : Controller
    {
        private readonly IRepositorioOrdenCompra repositorioOrdenCompra;
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        public OrdenCompraController(IRepositorioOrdenCompra repositorioOrdenCompra, IRepositorioMateriaPrima repositorioMateriaPrima)
        {
            this.repositorioOrdenCompra = repositorioOrdenCompra;
            this.repositorioMateriaPrima = repositorioMateriaPrima;
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(OrdenCompra orden)
        {
            await repositorioOrdenCompra.Crear(orden);
            var ordenes = await repositorioOrdenCompra.ObtenerOrdenCompras();
            return View("Index", ordenes);
        }


        public async Task<IActionResult> Index()//Listar las ordenes 
        {
            var ordenes = await repositorioOrdenCompra.ObtenerOrdenCompras();
            return View(ordenes);
        }
    }
}
