using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;

namespace MRPBaseDatosII.Controllers
{
    public class LaptopController : Controller
    {
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        //private readonly IRepositorioLaptop repositorioLaptop;
        public LaptopController(IRepositorioMateriaPrima repositorioMateriaPrima)
        {
            this.repositorioMateriaPrima = repositorioMateriaPrima;
            //this.repositorioLaptop = repositorioLaptop;
        }
        

        public async Task<IActionResult> Crear()
        {
            var materiasPrimas = await repositorioMateriaPrima.ObtenerStockCantidadTipo();
            var laptopConMateriaPrimaViewModel = new LaptopConMateriaPrimaViewModel
            {
                Laptop = new Laptop(),
                MateriasPrimas = materiasPrimas.ToList()
            };
            return View(laptopConMateriaPrimaViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
