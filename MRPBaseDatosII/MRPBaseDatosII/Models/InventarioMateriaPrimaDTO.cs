namespace MRPBaseDatosII.Models
{
    public class InventarioMateriaPrimaDTO
    {
        public string nombre {  get; set; }
        public string tipo { get; set; }
        public decimal precioUnitario { get; set; }
        public DateTime fechaCompra {  get; set; }
        public int stock { get; set; }
        public int stock_minimo { get; set; }
        public string ubicacion_seccion { get; set; }
        public string ubicacion_stand { get; set ; }

    }
}
