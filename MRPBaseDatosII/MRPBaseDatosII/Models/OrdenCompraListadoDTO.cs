namespace MRPBaseDatosII.Models
{
    public class OrdenCompraListadoDTO
    {
        public int Id { get; set; }
        public string MateriaPrima { get; set; }
        public string Proveedor { get; set; }
        public int CantidadUnidades { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
