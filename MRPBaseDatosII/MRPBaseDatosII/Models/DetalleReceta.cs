namespace MRPBaseDatosII.Models
{
    public class DetalleReceta
    {
        public int Id { get; set; }
        public int IdReceta { get; set; }
        public int IdMateriaPrima { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoTotalMateriaPrima { get; set; }
    }
}
