namespace MRPBaseDatosII.Models
{
    public class InventarioLaptopDTO
    {
        public string nombre { get; set; }
        public string tipo { get; set; }
        public decimal precioVenta { get; set; }
        public int stock { get; set; }
        public int stock_minimo { get; set; }
        public string ubicacion_seccion { get; set; }
        public string ubicacion_stand { get; set; }
    }
}
