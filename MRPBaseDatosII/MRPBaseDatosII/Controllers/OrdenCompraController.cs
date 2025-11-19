using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class OrdenCompraController : Controller
    {
        private readonly IRepositorioOrdenCompra repositorioOrdenCompra;
        private readonly IRepositorioProveedor repositorioProveedor;
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        public OrdenCompraController(IRepositorioOrdenCompra repositorioOrdenCompra, IRepositorioMateriaPrima repositorioMateriaPrima,IRepositorioProveedor repositorioProveedor)
        {
            this.repositorioOrdenCompra = repositorioOrdenCompra;
            this.repositorioMateriaPrima = repositorioMateriaPrima;
            this.repositorioProveedor = repositorioProveedor;
        }

        public async Task<IActionResult> Crear()
        {
            var proveedor = await repositorioProveedor.ObtenerNombreId();
            var materia = await repositorioMateriaPrima.ObtenerNombreId();
            var modelo = new OrdenCompraViewModel
            {
                ordenCompra = new OrdenCompra(),
                materiasPrimas = materia,
                proveedores = proveedor
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(OrdenCompra orden)//cambiar modelo a OrdenCompraViewModel si hay error
        {//luego cambiar de orden a modelo.ordenCompra
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
