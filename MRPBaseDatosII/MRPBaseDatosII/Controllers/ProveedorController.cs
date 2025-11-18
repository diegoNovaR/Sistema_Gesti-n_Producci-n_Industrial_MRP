using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IRepositorioProveedor repositorioProveedor;
        public ProveedorController(IRepositorioProveedor repositorioProveedor)
        {
            this.repositorioProveedor = repositorioProveedor;
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            var nuevoProveedor = await repositorioProveedor.Crear(proveedor);
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
