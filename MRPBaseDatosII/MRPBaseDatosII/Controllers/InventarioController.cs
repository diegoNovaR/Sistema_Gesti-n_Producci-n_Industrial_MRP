using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class InventarioController : Controller
    {
        public readonly IRepositorioInventario repositorioInventario;
        public InventarioController(IRepositorioInventario repositorioInventario)
        {
            this.repositorioInventario = repositorioInventario;
        }

        public async Task<IActionResult> InventarioLaptop()
        {
            var laptopInventario = await repositorioInventario.LaptopConInventario();
            return View(laptopInventario);
        }


        public async Task<IActionResult> InventarioMateriaPrima()
        {
            var materiaPrima = await repositorioInventario.MateriaPrimaConInventario();
            return View(materiaPrima);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
