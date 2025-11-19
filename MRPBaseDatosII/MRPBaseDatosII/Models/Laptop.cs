namespace MRPBaseDatosII.Models
{
    public class Laptop
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal CostoProduccion { get; set; }
    }
}
