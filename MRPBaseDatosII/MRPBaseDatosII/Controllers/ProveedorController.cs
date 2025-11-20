using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;
using System.Threading.Tasks;

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
            var modelo = new Proveedor();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            var nuevoProveedor = await repositorioProveedor.Crear(proveedor);
            var listaProveedor = await repositorioProveedor.ObtenerProveedores();
            return RedirectToAction("Index", listaProveedor);
        }


        public async Task<IActionResult> Index()
        {
            var modelo = await repositorioProveedor.ObtenerProveedores();
            return View(modelo);
        }
    }
}
