namespace MRPBaseDatosII.Models
{
    public class OrdenCompra
    {
        public int IdMateriaPrima { get; set; }
        public int IdProveedor { get; set; }
        public int CantidadUnidades { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
