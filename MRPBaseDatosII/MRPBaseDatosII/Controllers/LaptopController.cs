using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;
using MRPBaseDatosII.Servicios;
using Newtonsoft.Json;

namespace MRPBaseDatosII.Controllers
{
    public class LaptopController : Controller
    {
        private readonly IRepositorioMateriaPrima repositorioMateriaPrima;
        private readonly IRepositorioLaptop repositorioLaptop;
        public LaptopController(IRepositorioMateriaPrima repositorioMateriaPrima, IRepositorioLaptop repositorioLaptop)
        {
            this.repositorioMateriaPrima = repositorioMateriaPrima;
            this.repositorioLaptop = repositorioLaptop;
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

        [HttpPost]
        public async Task<IActionResult> Crear(LaptopConMateriaPrimaViewModel laptopConMateriaPrima)
        {
            if (!ModelState.IsValid)
            {
                var materiasPrimas = await repositorioMateriaPrima.ObtenerStockCantidadTipo();
                laptopConMateriaPrima.MateriasPrimas = materiasPrimas.ToList();
                return View(laptopConMateriaPrima);
            }

            var detallesReceta = new[]
            {
                new{id_materia = laptopConMateriaPrima.ProcesadorId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.RamId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.AlmacenamientoId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.PlacaMadreId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.PantallaId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.TarjetaVideoId, cantidad = 1},
                new {id_materia = laptopConMateriaPrima.ChasisId, cantidad = 1}
            };

            var jsonDetalles = JsonConvert.SerializeObject(detallesReceta);
            var result = await repositorioLaptop.Crear(laptopConMateriaPrima.Laptop, jsonDetalles);
            if (!result)
            {
                TempData["Error"] = "Hubo un error al crear la laptop";
                return RedirectToAction("Crear");
            }

            return RedirectToAction("Index");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
