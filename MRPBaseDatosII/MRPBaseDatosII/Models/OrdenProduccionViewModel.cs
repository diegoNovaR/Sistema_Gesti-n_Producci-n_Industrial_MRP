namespace MRPBaseDatosII.Models
{
    public class OrdenProduccionViewModel
    {
        public int Id { get; set; }
        public string nombreLaptop { get; set; }

    }

    public class CrearOrdenProduccionViewModel
    {
        public int LaptopId { get; set; }
        public int Cantidad { get; set; }

        public List<OrdenProduccionViewModel> Laptops { get; set; }
    }
}
