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
        public async Task<IActionResult> Crear(LaptopConMateriaPrimaViewModel model)
        {
            // Validar modelo
            if (!ModelState.IsValid)
            {
                model.MateriasPrimas = (await repositorioMateriaPrima.ObtenerStockCantidadTipo()).ToList();
                return View(model);
            }

            try
            {

                var detallesRecetaList = new List<dynamic>();

                // Componentes que siempre son necesarios:
                detallesRecetaList.Add(new { id_materia = model.ProcesadorId, cantidad = 1 });
                detallesRecetaList.Add(new { id_materia = model.RamId, cantidad = 1 });
                detallesRecetaList.Add(new { id_materia = model.AlmacenamientoId, cantidad = 1 });
                detallesRecetaList.Add(new { id_materia = model.PlacaMadreId, cantidad = 1 });
                detallesRecetaList.Add(new { id_materia = model.PantallaId, cantidad = 1 });
                detallesRecetaList.Add(new { id_materia = model.ChasisId, cantidad = 1 });

                var jsonDetalles = JsonConvert.SerializeObject(detallesRecetaList);

                if (model.TarjetaVideoId > 0)
                {
                    detallesRecetaList.Add(new { id_materia = model.TarjetaVideoId, cantidad = 1 });
                }

                // Llamada al repositorio
                var result = await repositorioLaptop.Crear(model.Laptop, jsonDetalles);

                if (!result)
                {
                    TempData["Error"] = "La función devolvió FALSE. No se pudo crear la laptop.";
                    return RedirectToAction("Crear");
                }

                TempData["Success"] = "Laptop creada exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = "ERROR BD: " + ex.Message;

                // Recargar lista de materias primas para reconstruir formulario
                model.MateriasPrimas = (await repositorioMateriaPrima.ObtenerStockCantidadTipo()).ToList();

                return View(model);
            }
        }



        public async Task<IActionResult> Index()
        {
            var laptops = await repositorioLaptop.ObtenerLaptops();
            return View("Index", laptops);
        }
    }
}
